using System.Windows;

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
                PreferredPropertyType = TxtPreferredType.Text
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
