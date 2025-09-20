using Real_Estate_Agencies.Model;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class EditClientWindow : Window
    {
        public EditClientWindow()
        {
            InitializeComponent();
        }

        // Load client data into the form
        public void LoadClient(ClientModel client)
        {
            if (client == null) return;

            TxtClientId.Text = client.ClientId.ToString();
            TxtFirstName.Text = client.FirstName;
            TxtLastName.Text = client.LastName;
            TxtContactInfo.Text = client.ContactInfo;
            TxtAddress.Text = client.Address;

            // Select preferred property type in ComboBox
            foreach (var item in CmbPropertyType.Items)
            {
                if (item is ComboBoxItem comboItem && comboItem.Content.ToString() == client.PreferredPropertyType)
                {
                    CmbPropertyType.SelectedItem = comboItem;
                    break;
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // Example validation
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                MessageBox.Show("First and Last Name are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update logic here
            ClientModel updatedClient = new ClientModel
            {
                ClientId = int.Parse(TxtClientId.Text),
                FirstName = TxtFirstName.Text,
                LastName = TxtLastName.Text,
                ContactInfo = TxtContactInfo.Text,
                Address = TxtAddress.Text,
                PreferredPropertyType = (CmbPropertyType.SelectedItem as ComboBoxItem)?.Content.ToString()
            };

            // Call your database update method
            // Database.UpdateClient(updatedClient);

            MessageBox.Show("Client updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
