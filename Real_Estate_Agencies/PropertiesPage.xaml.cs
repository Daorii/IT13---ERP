using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Views;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class PropertiesPage : Page
    {
        public PropertiesViewModel ViewModel { get; set; }

        public PropertiesPage()
        {
            InitializeComponent();
            ViewModel = new PropertiesViewModel();
            DataContext = ViewModel;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddPropertyWindow();
            if (addWindow.ShowDialog() == true)
            {
                ViewModel.Properties.Add(addWindow.NewProperty);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (PropertiesList.SelectedItem is PropertyModel selected)
            {
                var editWindow = new AddPropertyWindow
                {
                    TxtPropertyName = { Text = selected.Name },
                    TxtLocation = { Text = selected.Location },
                    TxtPropertyType = { Text = selected.PropertyType },
                    TxtCategory = { Text = selected.Category },
                    TxtPrice = { Text = selected.Price.ToString() },
                    CmbStatus = { SelectedItem = selected.Status }
                };

                if (editWindow.ShowDialog() == true)
                {
                    int index = ViewModel.Properties.IndexOf(selected);
                    ViewModel.Properties[index] = editWindow.NewProperty;
                }
            }
            else
            {
                MessageBox.Show("Please select a property to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (PropertiesList.SelectedItem is PropertyModel selected)
            {
                var result = MessageBox.Show($"Delete {selected.Name}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    ViewModel.Properties.Remove(selected);
                }
            }
            else
            {
                MessageBox.Show("Please select a property to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
