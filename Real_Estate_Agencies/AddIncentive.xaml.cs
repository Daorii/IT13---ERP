using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Real_Estate_Agencies
{
    public partial class AddIncentive : Window
    {
        public Incentive NewIncentive { get; set; }

        // Simulated agent data — replace with DB fetch later
        private List<Agent> allAgents = new List<Agent>();
        private List<Agent> filteredAgents = new List<Agent>();

        private int currentAgentPage = 0;
        private const int agentsPerPage = 5;

        private string selectedAgentName = "";

        public AddIncentive()
        {
            InitializeComponent();
            TxtReleaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            // Load agents from DB
            var agentRepo = new AgentRepository();
            allAgents = agentRepo.GetAllAgents();

            // Start hidden for animation
            MainCard.Opacity = 0;
            MainCard.RenderTransform = new System.Windows.Media.ScaleTransform(0.8, 0.8);

            Loaded += AddIncentiveWindow_Loaded; // Attach loaded event
        }

        // ----------------------------
        // OPENING TRANSITION
        // ----------------------------
        private void AddIncentiveWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade animation
            var fadeAnim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            MainCard.BeginAnimation(OpacityProperty, fadeAnim);

            // Scale animation
            var scaleAnim = new DoubleAnimation(0.8, 1, TimeSpan.FromMilliseconds(300))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            var scaleTransform = MainCard.RenderTransform as System.Windows.Media.ScaleTransform;
            if (scaleTransform != null)
            {
                scaleTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, scaleAnim);
                scaleTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, scaleAnim);
            }
        }

        // ----------------------------
        // AGENT SEARCH WITH PAGINATION
        // ----------------------------
        private void AgentSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = AgentSearchBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(query))
            {
                AgentSuggestionsList.Visibility = Visibility.Collapsed;
                AgentPaginationPanel.Visibility = Visibility.Collapsed;
                return;
            }

            filteredAgents = allAgents
                .Where(a => ($"{a.FirstName} {a.LastName}").ToLower().Contains(query))
                .ToList();

            currentAgentPage = 0;
            ShowAgentPage();
        }

        private void ShowAgentPage()
        {
            var pageItems = filteredAgents
                .Skip(currentAgentPage * agentsPerPage)
                .Take(agentsPerPage)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToList();

            AgentSuggestionsList.ItemsSource = pageItems;
            AgentSuggestionsList.Visibility = pageItems.Any() ? Visibility.Visible : Visibility.Collapsed;
            AgentPaginationPanel.Visibility = filteredAgents.Count > agentsPerPage ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NextAgentPageButton_Click(object sender, RoutedEventArgs e)
        {
            if ((currentAgentPage + 1) * agentsPerPage < filteredAgents.Count)
            {
                currentAgentPage++;
                ShowAgentPage();
            }
        }

        private void PrevAgentPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentAgentPage > 0)
            {
                currentAgentPage--;
                ShowAgentPage();
            }
        }

        private void AgentSuggestionsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AgentSuggestionsList.SelectedItem != null)
            {
                selectedAgentName = AgentSuggestionsList.SelectedItem.ToString();
                AgentSearchBox.Text = selectedAgentName;
                AgentSuggestionsList.Visibility = Visibility.Collapsed;
                AgentPaginationPanel.Visibility = Visibility.Collapsed;
            }
        }

        // ----------------------------
        // BUTTON HANDLERS
        // ----------------------------
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var selectedAgent = allAgents.FirstOrDefault(a =>
                    $"{a.FirstName} {a.LastName}".Equals(AgentSearchBox.Text, StringComparison.OrdinalIgnoreCase));

                if (selectedAgent == null)
                {
                    MessageBox.Show("Please select a valid agent.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NewIncentive = new Incentive
                {
                    AgentId = selectedAgent.AgentId,
                    AgentName = $"{selectedAgent.FirstName} {selectedAgent.LastName}",
                    IncentiveType = (CmbIncentiveType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "",
                    Amount = decimal.Parse(TxtAmount.Text),
                    ReleaseDate = DateTime.Parse(TxtReleaseDate.Text)
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(AgentSearchBox.Text))
            {
                MessageBox.Show("Please select an agent.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CmbIncentiveType.SelectedItem == null)
            {
                MessageBox.Show("Please select an Incentive Type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(TxtAmount.Text, out _))
            {
                MessageBox.Show("Please enter a valid Amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
