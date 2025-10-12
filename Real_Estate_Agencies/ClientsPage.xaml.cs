using Real_Estate_Agencies.Data;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Real_Estate_Agencies
{
    public partial class ClientsPage : Page
    {
        private readonly ClientRepository _repo;
        private readonly SalesRepository _salesRepo;

        public ObservableCollection<Client> Clients { get; set; }
        private ObservableCollection<Client> AllClients { get; set; }

        public Client SelectedClient { get; set; }

        public ClientsPage()
        {
            InitializeComponent();

            _repo = new ClientRepository();
            _salesRepo = new SalesRepository();

            AllClients = new ObservableCollection<Client>(_repo.GetAllClients());
            Clients = new ObservableCollection<Client>(AllClients);

            ClientsDataGrid.ItemsSource = Clients;
            DataContext = this;

            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
        }

        #region ➕ Add Client
        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddClientWindow();
            if (addWindow.ShowDialog() == true && addWindow.NewClient != null)
            {
                _repo.AddClient(addWindow.NewClient);
                AllClients.Add(addWindow.NewClient);
                Clients.Add(addWindow.NewClient);
            }
        }
        #endregion

        #region ✏️ Edit Client
        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Client client)
            {
                SelectedClient = client;

                // Fill in form fields
                EditClientIdTextBox.Text = client.ClientId.ToString();
                EditFirstNameTextBox.Text = client.FirstName;
                EditLastNameTextBox.Text = client.LastName;
                EditContactInfoTextBox.Text = client.ContactInfo;
                EditAddressTextBox.Text = client.Address;

                // Show overlay and animate the white panel
                EditPopupOverlay.Visibility = Visibility.Visible;
                AnimatePopupOpen();
            }
            else
            {
                MessageBox.Show("Please select a client to edit.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateField(EditFirstNameTextBox, "First name")) return;
                if (!ValidateField(EditLastNameTextBox, "Last name")) return;
                if (!ValidateField(EditContactInfoTextBox, "Contact info")) return;

                if (SelectedClient != null)
                {
                    SelectedClient.FirstName = EditFirstNameTextBox.Text.Trim();
                    SelectedClient.LastName = EditLastNameTextBox.Text.Trim();
                    SelectedClient.ContactInfo = EditContactInfoTextBox.Text.Trim();
                    SelectedClient.Address = EditAddressTextBox.Text.Trim();

                    _repo.UpdateClient(SelectedClient);
                    ClientsDataGrid.Items.Refresh();

                    MessageBox.Show("✅ Client updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseEditPopup();
                }
                else
                {
                    MessageBox.Show("No client selected for editing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Error saving client: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseEditPopup_Click(object sender, RoutedEventArgs e) => CloseEditPopup();

        private void CloseEditPopup()
        {
            AnimatePopupClose();
        }

        private void ClearEditFields()
        {
            EditClientIdTextBox.Text = "";
            EditFirstNameTextBox.Text = "";
            EditLastNameTextBox.Text = "";
            EditContactInfoTextBox.Text = "";
            EditAddressTextBox.Text = "";
        }

        private bool ValidateField(TextBox textBox, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                MessageBox.Show($"{fieldName} is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region 🗑️ Delete Client
        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Client client)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete client {client.FirstName} {client.LastName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _repo.DeleteClient(client.ClientId);
                    AllClients.Remove(client);
                    Clients.Remove(client);
                }
            }
            else
            {
                MessageBox.Show("Please select a client to delete.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region 🔍 Search
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTextBox.Text.Trim().ToLower();
            Clients.Clear();

            var filtered = AllClients.Where(c =>
                c.FirstName.ToLower().Contains(query) ||
                c.LastName.ToLower().Contains(query) ||
                c.ContactInfo.ToLower().Contains(query) ||
                c.Address.ToLower().Contains(query));

            foreach (var client in filtered)
                Clients.Add(client);
        }
        #endregion

        #region 👤 Client Profile Popup
        private void ClientRow_Click(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Grid)?.DataContext is Client client)
            {
                SelectedClient = client;

                ProfileFullName.Text = $"{client.FirstName} {client.LastName}";
                ProfileContactInfo.Text = client.ContactInfo;
                ProfileAddress.Text = client.Address;

                var saleInfo = _salesRepo.GetClientSaleInfo(client.ClientId);

                ProfileBalance.Text = saleInfo.balance.ToString("C");
                ProfilePaymentType.Text = saleInfo.paymentType;
                ProfileStatus.Text = saleInfo.status;

                ClientDetailsOverlay.Visibility = Visibility.Visible;
            }
        }

        private void CloseProfilePopup_Click(object sender, RoutedEventArgs e)
        {
            ClientDetailsOverlay.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region ✨ Popup Animations
        private void AnimatePopupOpen()
        {
            if (EditClientPanel == null) return;

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
            var scaleUp = new DoubleAnimation(0.9, 1, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            EditClientPanel.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            EditClientPanel.RenderTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, scaleUp);
            EditClientPanel.RenderTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, scaleUp);
        }

        private void AnimatePopupClose()
        {
            if (EditClientPanel == null) return;

            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
            var scaleDown = new DoubleAnimation(1, 0.9, TimeSpan.FromMilliseconds(150))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            fadeOut.Completed += (s, e) =>
            {
                EditPopupOverlay.Visibility = Visibility.Collapsed;
                ClearEditFields();
            };

            EditClientPanel.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            EditClientPanel.RenderTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, scaleDown);
            EditClientPanel.RenderTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, scaleDown);
        }
        #endregion
    }
}
