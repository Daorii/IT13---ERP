using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Real_Estate_Agencies
{
    public partial class AddSaleWindow : Window
    {
        public Sale NewSale { get; private set; }
        private readonly PropertyRepository _propertyRepository;

        // --- Agent Pagination ---
        private List<string> allAgents = new List<string>
        {
            "Agent A", "Agent B", "Agent C", "Agent D", "Agent E",
            "Agent F", "Agent G", "Agent H", "Agent I", "Agent J"
        };
        private List<string> filteredAgents = new List<string>();
        private int currentAgentPage = 0;
        private const int agentsPerPage = 5;

        public AddSaleWindow()
        {
            InitializeComponent();

            _propertyRepository = new PropertyRepository();

            // Populate PaymentMode dropdown
            PaymentModeComboBox.Items.Add("One-time Payment");
            PaymentModeComboBox.Items.Add("Installment");
        }

        // ---------------------------
        // CLIENT SEARCH
        // ---------------------------
        private void ClientSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = ClientSearchBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(query))
            {
                ClientSuggestionsList.Visibility = Visibility.Collapsed;
                return;
            }

            // TODO: Replace with DB query
            var allClients = new List<string>
            {
                "John Smith", "Mary Johnson", "Michael Brown",
                "Jane Williams", "Lucas Davis", "Sophia Miller"
            };

            var matches = allClients.Where(c => c.ToLower().Contains(query)).ToList();
            ClientSuggestionsList.ItemsSource = matches;
            ClientSuggestionsList.Visibility = matches.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClientSuggestionsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ClientSuggestionsList.SelectedItem != null)
            {
                ClientSearchBox.Text = ClientSuggestionsList.SelectedItem.ToString();
                ClientSuggestionsList.Visibility = Visibility.Collapsed;
            }
        }

        // ---------------------------
        // AGENT SEARCH WITH PAGINATION
        // ---------------------------
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

            AgentPaginationPanel.Visibility =
                filteredAgents.Count > agentsPerPage ? Visibility.Visible : Visibility.Collapsed;
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
                AgentSearchBox.Text = AgentSuggestionsList.SelectedItem.ToString();
                AgentSuggestionsList.Visibility = Visibility.Collapsed;
                AgentPaginationPanel.Visibility = Visibility.Collapsed;
            }
        }

        // ---------------------------
        // SAVE / CANCEL
        // ---------------------------
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate entries
            if (string.IsNullOrWhiteSpace(ClientSearchBox.Text))
            {
                MessageBox.Show("Please select a client.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(AgentSearchBox.Text))
            {
                MessageBox.Show("Please select an agent.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!SaleDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Please pick a sale date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string paymentMode = (PaymentModeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrWhiteSpace(paymentMode) || paymentMode.Contains("Select"))
            {
                MessageBox.Show("Please select a payment mode.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create Sale
            NewSale = new Sale
            {
                // For now, just store text names instead of IDs
                SaleDate = SaleDatePicker.SelectedDate.Value,
                PaymentMode = paymentMode
            };

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
