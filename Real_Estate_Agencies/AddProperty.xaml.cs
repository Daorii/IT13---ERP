using Microsoft.Win32;
using Real_Estate_Agencies.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Real_Estate_Agencies
{
    public partial class AddPropertyWindow : Window
    {
        public PropertyModel NewProperty { get; set; }

        public AddPropertyWindow()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (dlg.ShowDialog() == true)
            {
                TxtImagePath.Text = dlg.FileName;
                PreviewImage.Source = new BitmapImage(new Uri(dlg.FileName));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPropertyName.Text))
            {
                MessageBox.Show("Property Name is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewProperty = new PropertyModel
            {
                Name = TxtPropertyName.Text,
                Location = TxtLocation.Text,
                PropertyType = TxtPropertyType.Text,
                Category = TxtCategory.Text,
                Price = decimal.TryParse(TxtPrice.Text, out decimal p) ? p : 0,
                Status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "On Sale",
                ImagePath = string.IsNullOrEmpty(TxtImagePath.Text) ? "Images/default.jpg" : TxtImagePath.Text
            };

            this.DialogResult = true;
            this.Close();
        }
    }
}
