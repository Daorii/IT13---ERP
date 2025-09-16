using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class PropertiesPage : Page
    {
        public ObservableCollection<PropertyModel> Properties { get; set; }

        public PropertiesPage()
        {
            InitializeComponent();

            Properties = new ObservableCollection<PropertyModel>();
            PropertiesGrid.ItemsSource = Properties;
        }

        private void AddPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            AddPropertyWindow addWindow = new AddPropertyWindow();
            if (addWindow.ShowDialog() == true)
            {
                Properties.Add(addWindow.NewProperty);
            }
        }
    }

    public class PropertyModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
        public string Status { get; set; }
    }
}
