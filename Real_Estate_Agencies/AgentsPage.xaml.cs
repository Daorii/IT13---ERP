using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class AgentsPage : Page
    {
        private readonly AgentRepository _repo;

        public ObservableCollection<Agent> Agents { get; set; }

        private Agent SelectedAgent; // for editing

        public AgentsPage()
        {
            InitializeComponent();

            _repo = new AgentRepository();
            Agents = new ObservableCollection<Agent>(_repo.GetAllAgents());

            DataContext = this;
        }

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {
            AddAgentWindow addAgentWindow = new AddAgentWindow();
            if (addAgentWindow.ShowDialog() == true)
            {
                var newAgent = addAgentWindow.NewAgent;
                if (newAgent != null)
                {
                    _repo.AddAgent(newAgent);
                    Agents.Add(newAgent);
                }
            }
        }

        private void EditAgent_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Agent agent)
            {
                SelectedAgent = agent;

                // Fill form fields
                TxtAgentId.Text = agent.AgentId.ToString();
                TxtFirstName.Text = agent.FirstName;
                TxtLastName.Text = agent.LastName;
                TxtContact.Text = agent.ContactInfo;

                // Convert string to DateTime for DatePicker
                if (DateTime.TryParse(agent.HireDate, out DateTime hireDate))
                {
                    DpHireDate.SelectedDate = hireDate;
                }
                else
                {
                    DpHireDate.SelectedDate = DateTime.Today; // Default to today if invalid
                }

                // Show overlay
                EditAgentOverlay.Visibility = Visibility.Visible;
            }
        }

        private void DeleteAgent_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Agent agent)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete agent {agent.FirstName} {agent.LastName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repo.DeleteAgent(agent.AgentId);
                        Agents.Remove(agent);

                        MessageBox.Show("Agent deleted successfully.",
                            "Deleted",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting agent: {ex.Message}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            var filtered = _repo.GetAllAgents()
                .Where(a =>
                    a.FirstName.ToLower().Contains(searchText) ||
                    a.LastName.ToLower().Contains(searchText) ||
                    a.AgentId.ToString().Contains(searchText))
                .ToList();

            Agents.Clear();
            foreach (var a in filtered)
                Agents.Add(a);
        }

        private void CloseEditAgentOverlay_Click(object sender, RoutedEventArgs e)
        {
            EditAgentOverlay.Visibility = Visibility.Collapsed;
        }

        private void SaveAgent_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAgent != null)
            {
                // Update the selected agent from form fields
                SelectedAgent.FirstName = TxtFirstName.Text.Trim();
                SelectedAgent.LastName = TxtLastName.Text.Trim();
                SelectedAgent.ContactInfo = TxtContact.Text.Trim();

                // Convert DateTime to string (yyyy-MM-dd format)
                if (DpHireDate.SelectedDate.HasValue)
                {
                    SelectedAgent.HireDate = DpHireDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }

                // Update repository
                _repo.UpdateAgent(SelectedAgent);

                // Refresh ObservableCollection
                int index = Agents.IndexOf(SelectedAgent);
                Agents.RemoveAt(index);
                Agents.Insert(index, SelectedAgent);
            }

            EditAgentOverlay.Visibility = Visibility.Collapsed;
        }
        private void ViewAgentDetails_Click(object sender, RoutedEventArgs e)
{
    if ((sender as Button)?.CommandParameter is Agent agent)
    {
        // Populate the overlay details
        DetailsFullName.Text = $"{agent.FirstName} {agent.LastName}";
        DetailsContact.Text = agent.ContactInfo;
        DetailsAgentId.Text = agent.AgentId.ToString();
        DetailsAddress.Text = agent.Address; // Make sure your Agent model has Address
        DetailsHireDate.Text = agent.HireDate;
        DetailsUnitSales.Text = agent.NumberOfUnitSales.ToString(); // Add property in Agent class
        DetailsCommissionBalance.Text = $"₱ {agent.CommissionBalance:N2}"; // Add property in Agent class

        // Show image if exists
        if (agent.ProfileBitmap != null)
            DetailsProfileImage.Source = agent.ProfileBitmap;

        // Show overlay
        AgentDetailsOverlay.Visibility = Visibility.Visible;
    }
}

private void CloseAgentDetailsOverlay_Click(object sender, RoutedEventArgs e)
{
    AgentDetailsOverlay.Visibility = Visibility.Collapsed;
}
    }
}