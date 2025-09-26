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
        public PropertyModel SelectedProperty { get; set; }

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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is PropertyModel property)
            {
                var result = MessageBox.Show(
                    $"Delete {property.Name}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ViewModel.Properties.Remove(property);
                }
            }
            else
            {
                MessageBox.Show("Please select a property to delete.",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is PropertyModel property)
            {
                ViewModel.SelectedProperty = property;
                PopupOverlay.Visibility = Visibility.Visible; // show overlay
            }
            else
            {
                MessageBox.Show("No property selected.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.CommandParameter is PropertyModel property)
            {
                SelectedProperty = property;

                // Populate the form fields with property data
                EditNameTextBox.Text = property.Name;
                EditLocationTextBox.Text = property.Location;
                EditPropertyTypeComboBox.Text = property.PropertyType;
                EditCategoryComboBox.Text = property.Category;
                EditPriceTextBox.Text = property.Price.ToString();

                EditPopupOverlay.Visibility = Visibility.Visible; // Show edit overlay
            }
            else
            {
                MessageBox.Show("No property selected.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SaveProperty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(EditNameTextBox.Text))
                {
                    MessageBox.Show("Property name is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditNameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditLocationTextBox.Text))
                {
                    MessageBox.Show("Location is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditLocationTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditPriceTextBox.Text))
                {
                    MessageBox.Show("Price is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditPriceTextBox.Focus();
                    return;
                }

                // Validate price format
                if (!decimal.TryParse(EditPriceTextBox.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Please enter a valid price.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    EditPriceTextBox.Focus();
                    return;
                }

                // If we have a selected property, update the properties
                if (SelectedProperty != null)
                {
                    SelectedProperty.Name = EditNameTextBox.Text.Trim();
                    SelectedProperty.Location = EditLocationTextBox.Text.Trim();
                    SelectedProperty.PropertyType = EditPropertyTypeComboBox.Text.Trim();
                    SelectedProperty.Category = EditCategoryComboBox.Text.Trim();
                    SelectedProperty.Price = (double)price;

                    MessageBox.Show("Property updated successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    // Close the overlay
                    EditPopupOverlay.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("No property selected for editing.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error saving property: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseEditPopup_Click(object sender, RoutedEventArgs e)
        {
            EditPopupOverlay.Visibility = Visibility.Collapsed; // Hide edit overlay
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            PopupOverlay.Visibility = Visibility.Collapsed; // hide view overlay
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Previous page functionality
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Next page functionality
        }
    }
}