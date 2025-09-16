using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class ClientsPage : Page
    {
        public ObservableCollection<Client> Clients { get; set; }

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
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addWindow = new AddClientWindow();
            if (addWindow.ShowDialog() == true)
            {
                Clients.Add(addWindow.NewClient);
            }
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
