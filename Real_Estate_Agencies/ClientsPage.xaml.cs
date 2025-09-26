using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class ClientsPage : Page
    {
        public ObservableCollection<Client> Clients { get; set; }
        public Client SelectedClient { get; set; }

        public ClientsPage()
        {
            InitializeComponent();
            // Example data
            Clients = new ObservableCollection<Client>
            {
                new Client { ClientId = 1, FirstName = "John", LastName = "Doe", ContactInfo = "09123456789", Address = "123 Main St", PreferredPropertyType = "House" },
                new Client { ClientId = 2, FirstName = "Jane", LastName = "Smith", ContactInfo = "09987654321", Address = "456 Elm St", PreferredPropertyType = "Condo" }
            };
            ClientsDataGrid.ItemsSource = Clients;
            DataContext = this;
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addWindow = new AddClientWindow();
            if (addWindow.ShowDialog() == true)
            {
                Clients.Add(addWindow.NewClient);
            }
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Client client)
            {
                SelectedClient = client;

                // Populate the form fields with client data
                EditClientIdTextBox.Text = client.ClientId.ToString();
                EditFirstNameTextBox.Text = client.FirstName;
                EditLastNameTextBox.Text = client.LastName;
                EditContactInfoTextBox.Text = client.ContactInfo;
                EditAddressTextBox.Text = client.Address;

                // Set ComboBox selection
                EditPreferredPropertyTypeComboBox.Text = client.PreferredPropertyType;

                EditPopupOverlay.Visibility = Visibility.Visible; // Show overlay
            }
            else
            {
                MessageBox.Show("No client selected.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Client client)
            {
                var result = MessageBox.Show(
                    $"Delete client {client.FirstName} {client.LastName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Clients.Remove(client);
                }
            }
            else
            {
                MessageBox.Show("Please select a client to delete.",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void SaveClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(EditFirstNameTextBox.Text))
                {
                    MessageBox.Show("First name is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditFirstNameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditLastNameTextBox.Text))
                {
                    MessageBox.Show("Last name is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditLastNameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditContactInfoTextBox.Text))
                {
                    MessageBox.Show("Contact info is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditContactInfoTextBox.Focus();
                    return;
                }

                // If we have a selected client, update the properties
                if (SelectedClient != null)
                {
                    SelectedClient.FirstName = EditFirstNameTextBox.Text.Trim();
                    SelectedClient.LastName = EditLastNameTextBox.Text.Trim();
                    SelectedClient.ContactInfo = EditContactInfoTextBox.Text.Trim();
                    SelectedClient.Address = EditAddressTextBox.Text.Trim();
                    SelectedClient.PreferredPropertyType = EditPreferredPropertyTypeComboBox.Text.Trim();

                    MessageBox.Show("Client updated successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    // Refresh the ItemsControl to reflect changes
                    ClientsDataGrid.ItemsSource = null;
                    ClientsDataGrid.ItemsSource = Clients;

                    // Close the overlay
                    EditPopupOverlay.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("No client selected for editing.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error saving client: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseEditPopup_Click(object sender, RoutedEventArgs e)
        {
            EditPopupOverlay.Visibility = Visibility.Collapsed; // Hide overlay
        }
    }

    public class Client
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactInfo { get; set; }
        public string Address { get; set; }
        public string PreferredPropertyType { get; set; }
    }
}