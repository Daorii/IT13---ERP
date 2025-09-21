using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.ViewModels;
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
            if (ViewModel.SelectedProperty != null)
            {
                var editWindow = new AddPropertyWindow();
                editWindow.NewProperty = new PropertyModel
                {
                    Name = ViewModel.SelectedProperty.Name,
                    Location = ViewModel.SelectedProperty.Location,
                    PropertyType = ViewModel.SelectedProperty.PropertyType,
                    Category = ViewModel.SelectedProperty.Category,
                    Price = ViewModel.SelectedProperty.Price,
                    Status = ViewModel.SelectedProperty.Status,
                    ImagePath = ViewModel.SelectedProperty.ImagePath
                };

                if (editWindow.ShowDialog() == true)
                {
                    int index = ViewModel.Properties.IndexOf(ViewModel.SelectedProperty);
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
            if (ViewModel.SelectedProperty != null)
            {
                var result = MessageBox.Show($"Delete {ViewModel.SelectedProperty.Name}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    ViewModel.Properties.Remove(ViewModel.SelectedProperty);
                }
            }
            else
            {
                MessageBox.Show("Please select a property to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            // View property functionality
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            // Previous page functionality
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            // Next page functionality
        }
    }
}
