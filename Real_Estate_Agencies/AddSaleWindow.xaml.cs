using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Animation;

using System.Windows.Input;



namespace Real_Estate_Agencies
{
    public partial class AddSaleWindow : Window
    {

        public Sale NewSale { get; private set; }
        private readonly PropertyRepository _propertyRepository;


        private readonly ClientRepository _clientRepository;
        private List<Client> allClients;
        private List<PropertyModel> filteredProperties; // properties for the selected client

        private readonly AgentRepository _agentRepository;
        private List<Agent> allAgents;




        public AddSaleWindow()
        {
            InitializeComponent();

            _clientRepository = new ClientRepository();
            _propertyRepository = new PropertyRepository();
            _agentRepository = new AgentRepository();   // initialize here

            allClients = _clientRepository.GetAllClients();
            allAgents = _agentRepository.GetAllAgents(); // get all agents
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

            var matches = allClients
                .Where(c => $"{c.FirstName} {c.LastName}".ToLower().Contains(query))
                .Select(c => $"{c.FirstName} {c.LastName}")
                .ToList();

            ClientSuggestionsList.ItemsSource = matches;
            ClientSuggestionsList.Visibility = matches.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClientSuggestionsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ClientSuggestionsList.SelectedItem != null)
            {
                string selectedClientName = ClientSuggestionsList.SelectedItem.ToString();
                ClientSearchBox.Text = selectedClientName;
                ClientSuggestionsList.Visibility = Visibility.Collapsed;

                // Load properties for this client
                var selectedClient = allClients.FirstOrDefault(c => $"{c.FirstName} {c.LastName}" == selectedClientName);
                if (selectedClient != null)
                    // Enable PropertyNameComboBox
                    PropertySearchBox.IsEnabled = true;
                PropertySearchBox.Text = "";
                filteredProperties = _propertyRepository.GetAll().ToList();
                PropertySuggestionsList.ItemsSource = filteredProperties.Select(p => p.Name).ToList();
                PropertySuggestionsList.Visibility = Visibility.Collapsed;
                LblPrice.Content = "N/A";



                PropertySuggestionsList.ItemsSource = filteredProperties.Select(p => p.Name).ToList();
                PropertySuggestionsList.Visibility = Visibility.Collapsed;
                PropertySearchBox.Text = "";
                LblPrice.Content = "N/A";


                {

                }
            }
        }













        // ---------------------------
        // AGENT SEARCH WITH PAGINATION
        // ---------------------------
        private void AgentSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = AgentSearchBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(query))
            {
                AgentSuggestionsList.Visibility = Visibility.Collapsed;
                return;
            }

            var matches = allAgents
                .Where(a => $"{a.FirstName} {a.LastName}".ToLower().Contains(query))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToList();

            AgentSuggestionsList.ItemsSource = matches;
            AgentSuggestionsList.Visibility = matches.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AgentSuggestionsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AgentSuggestionsList.SelectedItem == null) return;

            AgentSearchBox.Text = AgentSuggestionsList.SelectedItem.ToString();
            AgentSuggestionsList.Visibility = Visibility.Collapsed;
        }


        private void AddSaleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var sb = new Storyboard();

            // Fade-in
            var opacityAnim = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            Storyboard.SetTarget(opacityAnim, MainCard);
            Storyboard.SetTargetProperty(opacityAnim, new PropertyPath(Border.OpacityProperty));
            sb.Children.Add(opacityAnim);

            // Scale X
            var scaleXAnim = new DoubleAnimation
            {
                From = 0.8,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(scaleXAnim, MainCard);
            Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("RenderTransform.ScaleX"));
            sb.Children.Add(scaleXAnim);

            // Scale Y
            var scaleYAnim = new DoubleAnimation
            {
                From = 0.8,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(scaleYAnim, MainCard);
            Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("RenderTransform.ScaleY"));
            sb.Children.Add(scaleYAnim);

            sb.Begin();
        }




        private void PropertySearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (filteredProperties == null || !PropertySearchBox.IsEnabled) return;

            string query = PropertySearchBox.Text.ToLower();
            var matches = filteredProperties
                .Where(p => p.Name.ToLower().Contains(query))
                .Select(p => p.Name)
                .ToList();

            PropertySuggestionsList.ItemsSource = matches;
            PropertySuggestionsList.Visibility = matches.Any() ? Visibility.Visible : Visibility.Collapsed;
        }


        private void PropertySuggestionsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PropertySuggestionsList.SelectedItem != null)
            {
                string selectedPropertyName = PropertySuggestionsList.SelectedItem.ToString();
                PropertySearchBox.Text = selectedPropertyName;
                PropertySuggestionsList.Visibility = Visibility.Collapsed;

                var selectedProperty = filteredProperties.FirstOrDefault(p => p.Name == selectedPropertyName);
                if (selectedProperty != null)
                {
                    LblPrice.Content = selectedProperty.Price.ToString("C");
                }
            }
        }







        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = allClients.FirstOrDefault(c => $"{c.FirstName} {c.LastName}" == ClientSearchBox.Text);
            if (selectedClient == null)
            {
                System.Windows.MessageBox.Show("Please select a valid client.", "Validation Error");
                return;
            }

            var selectedPropertyName = PropertySearchBox.Text;
            if (filteredProperties == null || string.IsNullOrEmpty(selectedPropertyName))
            {
                System.Windows.MessageBox.Show("Please select a valid property.", "Validation Error");
                return;
            }

            var selectedProperty = filteredProperties.FirstOrDefault(p => p.Name == selectedPropertyName);
            if (selectedProperty == null)
            {
                System.Windows.MessageBox.Show("Please select a valid property.", "Validation Error");
                return;
            }

            var selectedAgentName = AgentSearchBox.Text;
            var selectedAgent = allAgents.FirstOrDefault(a => $"{a.FirstName} {a.LastName}" == selectedAgentName);
            if (selectedAgent == null)
            {
                System.Windows.MessageBox.Show("Please select a valid agent.", "Validation Error");
                return;
            }

            // Create the sale with all necessary info
            NewSale = new Sale
            {
                ClientId = selectedClient.ClientId,
                ClientName = $"{selectedClient.FirstName} {selectedClient.LastName}", // store name for display
                PropertyId = selectedProperty.PropertyId,
                PropertyName = selectedProperty.Name,
                PropertyType = selectedProperty.PropertyType, // store type for display
                AgentId = selectedAgent.AgentId,
                AgentName = $"{selectedAgent.FirstName} {selectedAgent.LastName}", // store name for display
                SaleDate = SaleDatePicker.SelectedDate ?? DateTime.Now,
                PaymentMode = (PaymentModeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString()
            };

            // Save to database
            var salesRepo = new SalesRepository();


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
