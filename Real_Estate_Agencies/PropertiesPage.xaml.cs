using Microsoft.Data.SqlClient;
using Real_Estate_Agencies.Model;
using Real_Estate_Agencies.Repositories;
using Real_Estate_Agencies.ViewModels;
using Real_Estate_Agencies.Views;
using Real_Estate_Agencies.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Real_Estate_Agencies
{
    public partial class PropertiesPage : Page
    {
        public PropertiesViewModel ViewModel { get; set; }
        public PropertyModel SelectedProperty { get; set; }

        private readonly PropertyRepository _repository;

        private List<PropertyModel> allProperties; // Master list for search/filter

        private bool SalesExistForProperty(int propertyId)
        {
            using (var conn = DatabaseConnectionManager.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Sales WHERE PropertyID=@PropertyID";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PropertyID", propertyId);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public PropertiesPage()
        {
            InitializeComponent();
            _repository = new PropertyRepository();
            ViewModel = new PropertiesViewModel();
            DataContext = ViewModel;

            LoadPropertiesFromDatabase();

            // Initialize master list for search
            allProperties = ViewModel.Properties.ToList();

            // Attach search events
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            SearchTextBox.GotFocus += SearchTextBox_GotFocus;
            SearchTextBox.LostFocus += SearchTextBox_LostFocus;
        }

        private void LoadPropertiesFromDatabase()
        {
            try
            {
                var propertiesFromDb = _repository.GetAll();
                ViewModel.Properties = new ObservableCollection<PropertyModel>(propertiesFromDb);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading properties: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===================== SEARCH FUNCTION =====================
        private void ApplyFilter(string searchText)
        {
            searchText = searchText?.ToLower() ?? "";

            var filtered = string.IsNullOrWhiteSpace(searchText)
                ? allProperties
                : allProperties.Where(p =>
                    (p.Name != null && p.Name.ToLower().Contains(searchText)) ||
                    (p.Location != null && p.Location.ToLower().Contains(searchText)) ||
                    (p.PropertyType != null && p.PropertyType.ToLower().Contains(searchText)) ||
                    (p.Category != null && p.Category.ToLower().Contains(searchText)) ||
                    p.PropertyId.ToString().Contains(searchText)
                ).ToList();

            ViewModel.Properties.Clear();
            foreach (var prop in filtered)
                ViewModel.Properties.Add(prop);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter(SearchTextBox.Text);
            PlaceholderText.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
                PlaceholderText.Visibility = Visibility.Visible;
        }

        // ===================== EXISTING FUNCTIONS =====================
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddPropertyWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    _repository.Add(addWindow.NewProperty);
                    ViewModel.Properties.Add(addWindow.NewProperty);
                    allProperties.Add(addWindow.NewProperty);

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
                if (SalesExistForProperty(property.PropertyId))
                {
                    MessageBox.Show("Cannot delete this property because it has associated sales.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show(
                    $"Delete {property.Name}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repository.Delete(property.PropertyId);

                        // Remove by PropertyId instead of object instance
                        var itemToRemove = ViewModel.Properties.FirstOrDefault(p => p.PropertyId == property.PropertyId);
                        if (itemToRemove != null)
                        {
                            ViewModel.Properties.Remove(itemToRemove);
                            allProperties.Remove(itemToRemove);
                        }

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

                TxtPropertyId.Text = property.PropertyId.ToString();
                TxtPropertyName.Text = property.Name;

                // Fill Category and Type as read-only
                TxtCategory.Text = property.Category;      // New read-only field
                TxtPropertyType.Text = property.PropertyType; // New read-only field

                TxtAddress.Text = property.Location;
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
                    // Preserve Category and PropertyType from original property
                    SelectedProperty.Price = (double)price;
                    SelectedProperty.Status = CmbStatus.Text.Trim();

                    _repository.Update(SelectedProperty);

                    var propertyInCollection = ViewModel.Properties
                        .FirstOrDefault(p => p.PropertyId == SelectedProperty.PropertyId);

                    if (propertyInCollection != null)
                    {
                        int index = ViewModel.Properties.IndexOf(propertyInCollection);
                        ViewModel.Properties[index] = SelectedProperty;
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
