using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Real_Estate_Agencies
{
    public partial class ClientsPage : Page
    {
        private readonly ClientRepository _repo;
        public ObservableCollection<Client> Clients { get; set; }
        public Client SelectedClient { get; set; }

        public ClientsPage()
        {
            InitializeComponent();

            _repo = new ClientRepository();
            Clients = new ObservableCollection<Client>(_repo.GetAllClients());

            ClientsDataGrid.ItemsSource = Clients;
            DataContext = this;
        }

        #region Add / Edit / Delete Client

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addWindow = new AddClientWindow();
            if (addWindow.ShowDialog() == true && addWindow.NewClient != null)
            {
                _repo.AddClient(addWindow.NewClient);   // save to DB
                Clients.Add(addWindow.NewClient);       // update UI
            }
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Client client)
            {
                SelectedClient = client;

                // Populate form fields
                EditClientIdTextBox.Text = client.ClientId.ToString();
                EditFirstNameTextBox.Text = client.FirstName;
                EditLastNameTextBox.Text = client.LastName;
                EditContactInfoTextBox.Text = client.ContactInfo;
                EditAddressTextBox.Text = client.Address;
                EditPreferredPropertyTypeComboBox.Text = client.PreferredPropertyType;

                EditPopupOverlay.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("No client selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    _repo.DeleteClient(client.ClientId);   // delete from DB
                    Clients.Remove(client);                // update UI
                }
            }
            else
            {
                MessageBox.Show("Please select a client to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(EditFirstNameTextBox.Text))
                {
                    MessageBox.Show("First name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditFirstNameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditLastNameTextBox.Text))
                {
                    MessageBox.Show("Last name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditLastNameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditContactInfoTextBox.Text))
                {
                    MessageBox.Show("Contact info is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditContactInfoTextBox.Focus();
                    return;
                }

                if (SelectedClient != null)
                {
                    SelectedClient.FirstName = EditFirstNameTextBox.Text.Trim();
                    SelectedClient.LastName = EditLastNameTextBox.Text.Trim();
                    SelectedClient.ContactInfo = EditContactInfoTextBox.Text.Trim();
                    SelectedClient.Address = EditAddressTextBox.Text.Trim();
                    SelectedClient.PreferredPropertyType = EditPreferredPropertyTypeComboBox.Text.Trim();

                    _repo.UpdateClient(SelectedClient);

                    MessageBox.Show("Client updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    ClientsDataGrid.Items.Refresh();
                    EditPopupOverlay.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("No client selected for editing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving client: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseEditPopup_Click(object sender, RoutedEventArgs e)
        {
            EditPopupOverlay.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Profile Popup

        private void ClientRow_Click(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Grid)?.DataContext is Client client)
            {
                ProfileClientId.Text = client.ClientId.ToString();
                ProfileFullName.Text = $"{client.FirstName} {client.LastName}";
                ProfileContactInfo.Text = client.ContactInfo;
                ProfileAddress.Text = client.Address;
                ProfilePreferredProperty.Text = client.PreferredPropertyType;
                ProfileBalance.Text = client.Balance.ToString("C");
                ProfilePaymentDate.Text = client.PaymentDate.ToShortDateString();
                ProfileAmountPaid.Text = client.AmountPaid.ToString("C");
                ProfilePaymentType.Text = client.PaymentType;
                ProfileStatus.Text = client.Status;

                ProfilePopupOverlay.Visibility = Visibility.Visible;
            }
        }

        private void CloseProfilePopup_Click(object sender, RoutedEventArgs e)
        {
            ProfilePopupOverlay.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}
