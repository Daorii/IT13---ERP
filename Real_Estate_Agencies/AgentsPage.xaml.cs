using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Real_Estate_Agencies
{
    public partial class AgentsPage : Page
    {
        private readonly AgentRepository _repo;

        private List<Agent> allAgents; // Master list of agents
        public ObservableCollection<Agent> Agents { get; set; } // Bound to UI

        private Agent SelectedAgent; // for editing

        private readonly Dictionary<int, decimal> _currentBalances = new Dictionary<int, decimal>();
        private readonly Dictionary<int, decimal> _balanceResetOffset = new Dictionary<int, decimal>();

        private ICollectionView AgentsView; // For fast filtering
        private CancellationTokenSource _cts; // For debounce typing

        public AgentsPage()
        {
            InitializeComponent();

            _repo = new AgentRepository();
            LoadAgents();

            DataContext = this;

            // Setup CollectionView for fast filtering
            AgentsView = CollectionViewSource.GetDefaultView(Agents);
            AgentsView.Filter = FilterAgents;

            // Attach search event
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
        }

        private void LoadAgents()
        {
            allAgents = _repo.GetAllAgents();
            Agents = new ObservableCollection<Agent>(allAgents);
        }

        // Filter logic used by CollectionView
        private bool FilterAgents(object obj)
        {
            if (obj is Agent agent)
            {
                string searchText = SearchTextBox.Text?.ToLower() ?? "";
                if (string.IsNullOrWhiteSpace(searchText)) return true;

                return agent.FirstName.ToLower().Contains(searchText)
                       || agent.LastName.ToLower().Contains(searchText)
                       || agent.AgentId.ToString().Contains(searchText);
            }
            return false;
        }

        // Debounced search for smoother typing
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Cancel previous debounce
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Task.Delay(200).ContinueWith(t =>
            {
                if (!token.IsCancellationRequested)
                {
                    Dispatcher.Invoke(() =>
                    {
                        AgentsView.Refresh(); // Only refresh the view, no clearing/adding
                        PlaceholderText.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
                    });
                }
            });
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
                PlaceholderText.Visibility = Visibility.Visible;
        }

        // ---- Existing CRUD and agent detail methods stay the same ----

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {
            AddAgentWindow addAgentWindow = new AddAgentWindow();
            if (addAgentWindow.ShowDialog() == true)
            {
                var newAgent = addAgentWindow.NewAgent;
                if (newAgent != null)
                {
                    _repo.AddAgent(newAgent);
                    allAgents.Add(newAgent);
                    Agents.Add(newAgent);
                    AgentsView.Refresh();
                }
            }
        }

        private void EditAgent_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Agent agent)
            {
                SelectedAgent = agent;
                TxtAgentId.Text = agent.AgentId.ToString();
                TxtFirstName.Text = agent.FirstName;
                TxtLastName.Text = agent.LastName;
                TxtContact.Text = agent.ContactInfo;
                if (DateTime.TryParse(agent.HireDate, out DateTime hireDate))
                    DpHireDate.SelectedDate = hireDate;
                else
                    DpHireDate.SelectedDate = DateTime.Today;

                EditAgentOverlay.Visibility = Visibility.Visible;
            }
        }

        private void SaveAgent_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAgent != null)
            {
                SelectedAgent.FirstName = TxtFirstName.Text.Trim();
                SelectedAgent.LastName = TxtLastName.Text.Trim();
                SelectedAgent.ContactInfo = TxtContact.Text.Trim();
                if (DpHireDate.SelectedDate.HasValue)
                    SelectedAgent.HireDate = DpHireDate.SelectedDate.Value.ToString("yyyy-MM-dd");

                _repo.UpdateAgent(SelectedAgent);

                AgentsView.Refresh();
            }
            EditAgentOverlay.Visibility = Visibility.Collapsed;
        }

        private void DeleteAgent_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Agent agent)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete agent {agent.FirstName} {agent.LastName}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repo.DeleteAgent(agent.AgentId);
                        allAgents.Remove(agent);
                        Agents.Remove(agent);
                        AgentsView.Refresh();
                        MessageBox.Show("Agent deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting agent: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // ---- Agent detail overlay methods stay the same ----

        private void ViewAgentDetails_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Agent agent)
            {
                DetailsFullName.Text = $"{agent.FirstName} {agent.LastName}";
                DetailsContact.Text = agent.ContactInfo;
                DetailsAgentId.Text = agent.AgentId.ToString();
                DetailsHireDate.Text = agent.HireDate;

                try
                {
                    decimal totalBalance = _repo.GetAgentBalanceThisMonth(agent.AgentId);
                    if (_balanceResetOffset.TryGetValue(agent.AgentId, out decimal offset))
                        totalBalance -= offset;
                    totalBalance = Math.Max(totalBalance, 0);

                    DetailsUnitSales.Text = _repo.GetAgentSalesCountThisMonth(agent.AgentId).ToString();
                    DetailsCommissionBalance.Text = totalBalance.ToString("C", new CultureInfo("en-PH"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading agent stats: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    DetailsUnitSales.Text = "0";
                    DetailsCommissionBalance.Text = (0m).ToString("C", new CultureInfo("en-PH"));
                }

                if (agent.ProfileBitmap != null)
                    DetailsProfileImage.Source = agent.ProfileBitmap;

                AgentDetailsOverlay.Visibility = Visibility.Visible;
            }
        }

        private void CloseAgentDetailsOverlay_Click(object sender, RoutedEventArgs e)
        {
            AgentDetailsOverlay.Visibility = Visibility.Collapsed;
        }

        private void ReleaseCommission_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(DetailsAgentId.Text, out int agentId)) return;

            try
            {
                decimal totalBalance = _repo.GetAgentBalanceThisMonth(agentId);
                if (totalBalance <= 0)
                {
                    MessageBox.Show("No commission balance to release.", "Notice",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var releaseRepo = new CommissionReleaseRepository();
                releaseRepo.Add(agentId, totalBalance);

                DetailsCommissionBalance.Text = "₱0.00";
                MessageBox.Show("Commission released successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                AgentDetailsOverlay.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error releasing commission: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Close edit agent overlay
        private void CloseEditAgentOverlay_Click(object sender, RoutedEventArgs e)
        {
            EditAgentOverlay.Visibility = Visibility.Collapsed;
        }

    }
}
