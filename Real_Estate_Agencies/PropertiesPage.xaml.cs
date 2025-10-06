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

        private readonly PropertyRepository _repository;

        public PropertiesPage()
        {
            InitializeComponent();
            _repository = new PropertyRepository();
            ViewModel = new PropertiesViewModel();
            DataContext = ViewModel;

            LoadPropertiesFromDatabase();
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
                    _repository.Add(addWindow.NewProperty);
                    ViewModel.Properties.Add(addWindow.NewProperty);

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
                        _repository.Delete(property.Id);
                        ViewModel.Properties.Remove(property);
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
                PopupOverlay.Visibility = Visibility.Visible;
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

                TxtPropertyId.Text = property.Id.ToString();
                TxtPropertyName.Text = property.Name;
                TxtAddress.Text = property.Location;
                CmbPropertyType.Text = property.PropertyType;
                TxtPrice.Text = property.Price.ToString();
                CmbStatus.Text = property.Status;

                EditPropertyOverlay.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("No property selected.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void UpdateProperty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtPropertyName.Text))
                {
                    MessageBox.Show("Property name is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    TxtPropertyName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtAddress.Text))
                {
                    MessageBox.Show("Location is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    TxtAddress.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtPrice.Text))
                {
                    MessageBox.Show("Price is required.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    TxtPrice.Focus();
                    return;
                }

                if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Please enter a valid price.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    TxtPrice.Focus();
                    return;
                }

                if (SelectedProperty != null)
                {
                    SelectedProperty.Name = TxtPropertyName.Text.Trim();
                    SelectedProperty.Location = TxtAddress.Text.Trim();
                    SelectedProperty.PropertyType = CmbPropertyType.Text.Trim();
                    SelectedProperty.Price = (double)price;
                    SelectedProperty.Status = CmbStatus.Text.Trim();

                    _repository.Update(SelectedProperty);

                    int index = ViewModel.Properties.IndexOf(SelectedProperty);
                    if (index >= 0)
                    {
                        ViewModel.Properties.RemoveAt(index);
                        ViewModel.Properties.Insert(index, SelectedProperty);
                    }

                    MessageBox.Show("Property updated successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    EditPropertyOverlay.Visibility = Visibility.Collapsed;
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

        private void CloseEditOverlay_Click(object sender, RoutedEventArgs e)
        {
            EditPropertyOverlay.Visibility = Visibility.Collapsed;
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            PopupOverlay.Visibility = Visibility.Collapsed;
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