using Real_Estate_Agencies.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Real_Estate_Agencies
{
    public partial class AddIncentive : Window
    {
        public Incentive NewIncentive { get; set; }

        // Simulated agent data — replace with DB fetch later
        private List<string> allAgents = new List<string>
        {
            "Agent A", "Agent B", "Agent C", "Agent D", "Agent E",
            "Agent F", "Agent G", "Agent H", "Agent I", "Agent J"
        };
        private List<string> filteredAgents = new List<string>();
        private int currentAgentPage = 0;
        private const int agentsPerPage = 5;

        private string selectedAgentName = "";

        public AddIncentive()
        {
            InitializeComponent();
            TxtReleaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
                .Where(a => a.ToLower().Contains(query))
                .ToList();

            currentAgentPage = 0;
            ShowAgentPage();
        }

        private void ShowAgentPage()
        {
            var pageItems = filteredAgents
                .Skip(currentAgentPage * agentsPerPage)
                .Take(agentsPerPage)
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
                NewIncentive = new Incentive
                {
                    // For now, store Agent Name only (can map to AgentId later)
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
