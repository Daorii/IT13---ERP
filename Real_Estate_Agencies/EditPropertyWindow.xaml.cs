using System;
using System.Windows;
using Microsoft.Win32;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies.Views
{
    public partial class EditPropertyWindow : Window
    {
        public PropertyModel Property { get; set; }
        public bool IsUpdated { get; private set; } = false;

        public EditPropertyWindow(PropertyModel property)
        {
            InitializeComponent();
            Property = property ?? throw new ArgumentNullException(nameof(property));
            LoadPropertyData();
        }

        private void LoadPropertyData()
        {
            PropertyNameTextBox.Text = Property.Name;
            PropertyLocationTextBox.Text = Property.Location;
            PropertyTypeComboBox.Text = Property.PropertyType ?? "";
            CategoryComboBox.Text = Property.Category ?? "";
            PriceTextBox.Text = Property.Price.ToString("F2"); // decimal formatted with 2 decimals
            StatusComboBox.Text = Property.Status ?? "";
            ImagePathTextBox.Text = Property.ImagePath ?? "";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(PropertyNameTextBox.Text))
            {
                MessageBox.Show("Property Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                PropertyNameTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(PropertyLocationTextBox.Text))
            {
                MessageBox.Show("Property Location is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                PropertyLocationTextBox.Focus();
                return;
            }

            if (PropertyTypeComboBox.SelectedIndex == -1 || string.IsNullOrWhiteSpace(PropertyTypeComboBox.Text))
            {
                MessageBox.Show("Property Type is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                PropertyTypeComboBox.Focus();
                return;
            }

            if (CategoryComboBox.SelectedIndex == -1 || string.IsNullOrWhiteSpace(CategoryComboBox.Text))
            {
                MessageBox.Show("Category is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CategoryComboBox.Focus();
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                PriceTextBox.Focus();
                return;
            }

            if (StatusComboBox.SelectedIndex == -1 || string.IsNullOrWhiteSpace(StatusComboBox.Text))
            {
                MessageBox.Show("Status is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                StatusComboBox.Focus();
                return;
            }

            // Update property
            Property.Name = PropertyNameTextBox.Text.Trim();
            Property.Location = PropertyLocationTextBox.Text.Trim();
            Property.PropertyType = PropertyTypeComboBox.Text.Trim();
            Property.Category = CategoryComboBox.Text.Trim();
            Property.Price = (double)price; // Fixed: Cast decimal to double
            Property.Status = StatusComboBox.Text.Trim();
            Property.ImagePath = string.IsNullOrWhiteSpace(ImagePathTextBox.Text) ? "Images/default.jpg" : ImagePathTextBox.Text.Trim();

            IsUpdated = true;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsUpdated = false;
            DialogResult = false;
            Close();
        }

        private void BrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Property Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}