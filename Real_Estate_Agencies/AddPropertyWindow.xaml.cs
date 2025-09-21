using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Real_Estate_Agencies.Model;
using System.ComponentModel;

namespace Real_Estate_Agencies.Views
{
    public partial class AddPropertyWindow : Window
    {
        public PropertyModel NewProperty { get; set; }

        public AddPropertyWindow()
        {
            InitializeComponent();
            NewProperty = new PropertyModel();
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Force data binding update
            this.UpdateLayout();

            // Validate required fields
            if (string.IsNullOrWhiteSpace(NewProperty.Name) ||
                string.IsNullOrWhiteSpace(NewProperty.Location) ||
                string.IsNullOrWhiteSpace(NewProperty.PropertyType))
            {
                MessageBox.Show("Please fill in all required fields (Name, Location, Property Type).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate price
            if (NewProperty.Price <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than 0.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get selected status from ComboBox
            if (StatusComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                NewProperty.Status = selectedItem.Content.ToString();
            }
            else
            {
                NewProperty.Status = "On Sale"; // Default status
            }

            // Set default image if none selected
            if (string.IsNullOrWhiteSpace(NewProperty.ImagePath))
            {
                NewProperty.ImagePath = "Images/default.jpg";
            }

            // Generate a unique ID for the property
            NewProperty.Id = (int)DateTime.Now.Ticks; // Simple unique ID generation
            NewProperty.Code = $"P{DateTime.Now:yyyyMMddHHmmss}"; // Generate property code

            MessageBox.Show($"Property '{NewProperty.Name}' has been saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
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
                NewProperty.ImagePath = openFileDialog.FileName;
            }
        }
    }
}