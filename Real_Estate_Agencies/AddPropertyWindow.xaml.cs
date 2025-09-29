using Microsoft.Win32;
using Real_Estate_Agencies.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies.Views
{
    public partial class AddPropertyWindow : Window
    {
        public PropertyModel NewProperty { get; set; }

        // Category and Type collections
        public List<string> Categories { get; set; }
        public Dictionary<string, List<string>> CategoryTypes { get; set; }
        public List<string> FilteredPropertyTypes { get; set; }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                UpdateFilteredTypes();
            }
        }

        public AddPropertyWindow()
        {
            InitializeComponent();

            // Initialize property model
            NewProperty = new PropertyModel();

            // Initialize Categories
            Categories = new List<string>
            {
                "Residential", "Commercial", "Industrial", "Lot Only", "Agricultural", "Mixed-Use"
            };

            // Initialize mapping between Category -> Types
            CategoryTypes = new Dictionary<string, List<string>>
            {
                { "Residential", new List<string>{ "Condominium – Studio", "Condominium – 1BR", "Condominium – 2BR", "Condominium – 3BR", "Condominium – Penthouse", "Apartment Unit", "Townhouse", "Single-Detached House", "Duplex", "Rowhouse" } },
                { "Commercial", new List<string>{ "Commercial Space (Retail)", "Office Space", "Restaurant Space", "Showroom", "Co-Working Space" } },
                { "Industrial", new List<string>{ "Warehouse", "Factory", "Manufacturing Plant", "Distribution Center" } },
                { "Lot Only", new List<string>{ "Vacant Lot – Residential", "Vacant Lot – Commercial", "Vacant Lot – Mixed-Use" } },
                { "Agricultural", new List<string>{ "Farm Lot", "Plantation Land", "Agro-Industrial Lot" } },
                { "Mixed-Use", new List<string>{ "Residential + Commercial Building", "Commercial/Residential Lot", "Shop-Top Housing (store on ground floor, unit upstairs)" } }
            };

            // Default: empty types
            FilteredPropertyTypes = new List<string>();

            DataContext = this;
        }

        private void UpdateFilteredTypes()
        {
            if (!string.IsNullOrEmpty(SelectedCategory) && CategoryTypes.ContainsKey(SelectedCategory))
                FilteredPropertyTypes = CategoryTypes[SelectedCategory];
            else
                FilteredPropertyTypes = new List<string>();

            // Refresh TypeComboBox binding
            TypeComboBox.ItemsSource = FilteredPropertyTypes;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Assign category and type from ComboBoxes
            if (CategoryComboBox.SelectedItem != null)
                NewProperty.Category = CategoryComboBox.SelectedItem.ToString();

            if (TypeComboBox.SelectedItem != null)
                NewProperty.PropertyType = TypeComboBox.SelectedItem.ToString();

            // Validate required fields
            if (string.IsNullOrWhiteSpace(NewProperty.Name) ||
                string.IsNullOrWhiteSpace(NewProperty.Location) ||
                string.IsNullOrWhiteSpace(NewProperty.PropertyType) ||
                string.IsNullOrWhiteSpace(NewProperty.Category))
            {
                MessageBox.Show("Please fill in all required fields (Name, Location, Category, Property Type).",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate price
            if (NewProperty.Price <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than 0.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get selected status
            if (StatusComboBox.SelectedItem is ComboBoxItem selectedItem)
                NewProperty.Status = selectedItem.Content.ToString();
            else
                NewProperty.Status = "On Sale"; // Default

            // If no image selected, set null
            if (NewProperty.Image == null || NewProperty.Image.Length == 0)
            {
                MessageBox.Show("No image selected. A default image will be used.",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                NewProperty.Image = Array.Empty<byte>();
            }

            // Generate temporary ID
            NewProperty.Id = (int)DateTime.Now.Ticks & 0xFFFFFF;

            MessageBox.Show($"Property '{NewProperty.Name}' saved successfully!",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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
            Close();
        }

        private void BrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Property Image",
                Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog() == true)
            {
                NewProperty.Image = File.ReadAllBytes(openFileDialog.FileName);
                MessageBox.Show("Image loaded successfully!", "Image", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
