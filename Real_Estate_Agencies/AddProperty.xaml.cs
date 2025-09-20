using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies
{
    public partial class AddPropertyWindow : Window
    {
        public PropertyModel NewProperty { get; set; }

        public AddPropertyWindow()
        {
            InitializeComponent();
            NewProperty = new PropertyModel();
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Property Image",
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                TxtImagePath.Text = System.IO.Path.GetFileName(path);
                NewProperty.ImagePath = path;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                PreviewImage.Source = bitmap;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPropertyName.Text))
            {
                MessageBox.Show("Property Name is required.");
                TxtPropertyName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtLocation.Text))
            {
                MessageBox.Show("Location is required.");
                TxtLocation.Focus();
                return;
            }

            if (CmbPropertyType.SelectedItem == null)
            {
                MessageBox.Show("Property Type is required.");
                CmbPropertyType.Focus();
                return;
            }

            if (CmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Category is required.");
                CmbCategory.Focus();
                return;
            }

            if (!double.TryParse(TxtPrice.Text, out double price) || price <= 0)
            {
                MessageBox.Show("Valid price required.");
                TxtPrice.Focus();
                return;
            }

            if (CmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Status is required.");
                CmbStatus.Focus();
                return;
            }

            NewProperty.Name = TxtPropertyName.Text.Trim();
            NewProperty.Location = TxtLocation.Text.Trim();
            NewProperty.PropertyType = (CmbPropertyType.SelectedItem as ComboBoxItem)?.Content?.ToString();
            NewProperty.Category = (CmbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString();
            NewProperty.Price = price;
            NewProperty.Status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (string.IsNullOrEmpty(NewProperty.ImagePath))
                NewProperty.ImagePath = "Images/default.jpg";

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
