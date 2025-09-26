using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class AddClientWindow : Window
    {
        public Client NewClient { get; set; }

        public AddClientWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NewClient = new Client
            {
                ClientId = int.TryParse(TxtClientId.Text, out int id) ? id : 0,
                FirstName = TxtFirstName.Text,
                LastName = TxtLastName.Text,
                ContactInfo = TxtContactInfo.Text,
                Address = TxtAddress.Text,
                PreferredPropertyType = (TxtPreferredType.SelectedItem as ComboBoxItem)?.Content.ToString()
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
