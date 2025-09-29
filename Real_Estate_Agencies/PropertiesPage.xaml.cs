using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.ViewModels;
using Real_Estate_Agencies.Views;
using Real_Estate_Agencies.Repositories;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class PropertiesPage : Page
    {
        public PropertiesViewModel ViewModel { get; set; }
        public PropertyModel SelectedProperty { get; set; }

        private readonly PropertyRepository _repository; // repository instance

        public PropertiesPage()
        {
            InitializeComponent();
            _repository = new PropertyRepository(); // initialize repository
            ViewModel = new PropertiesViewModel();
            DataContext = ViewModel;

            LoadPropertiesFromDatabase(); // load existing properties
        }

        private void LoadPropertiesFromDatabase()
        {
            try
            {
                var propertiesFromDb = _repository.GetAll();
                ViewModel.Properties = new System.Collections.ObjectModel.ObservableCollection<PropertyModel>(propertiesFromDb);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading properties: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddPropertyWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    _repository.Add(addWindow.NewProperty); // save to database
                    ViewModel.Properties.Add(addWindow.NewProperty); // add to ObservableCollection

                    MessageBox.Show("Property added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding property: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                    try
                    {
                        _repository.Delete(property.Id); // delete from database
                        ViewModel.Properties.Remove(property); // remove from ObservableCollection
                        MessageBox.Show("Property deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting property: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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

                if (SelectedProperty != null)
                {
                    SelectedProperty.Name = EditNameTextBox.Text.Trim();
                    SelectedProperty.Location = EditLocationTextBox.Text.Trim();
                    SelectedProperty.PropertyType = EditPropertyTypeComboBox.Text.Trim();
                    SelectedProperty.Category = EditCategoryComboBox.Text.Trim();
                    SelectedProperty.Price = (double)price;

                    try
                    {
                        _repository.Update(SelectedProperty); // save changes to database
                        MessageBox.Show("Property updated successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        EditPopupOverlay.Visibility = Visibility.Collapsed; // close overlay
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating property: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No property selected for editing.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
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
