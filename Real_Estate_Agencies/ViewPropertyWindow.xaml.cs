using Real_Estate_Agencies.Model;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Real_Estate_Agencies.Views
{
    public partial class ViewPropertyWindow : Window
    {
        public PropertyModel Property { get; private set; }

        public ViewPropertyWindow(PropertyModel property)
        {
            InitializeComponent();

            if (property != null)
            {
                PropertyNameDisplay.Text = property.Name;
                PropertyLocationDisplay.Text = property.Location;
                PropertyTypeDisplay.Text = property.PropertyType; // or map to Type
                CategoryDisplay.Text = property.Category;
                PriceDisplay.Text = property.Price.ToString("C");
                PropertyNumberDisplay.Text = property.No.ToString(); // if you still want a number

                StatusText.Text = property.Status ?? "N/A";

                if (property.Image != null)
                {
                    using (var ms = new MemoryStream(property.Image))
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                        PropertyImage.Source = image;
                    }
                }
            }
        }


        private void LoadPropertyDetails()
        {
            if (Property == null) return;

            // Basic details
            PropertyNameDisplay.Text = Property.Name ?? "N/A";
            PropertyLocationDisplay.Text = Property.Location ?? "N/A";
            PropertyTypeDisplay.Text = Property.PropertyType ?? "N/A";
            CategoryDisplay.Text = Property.Category ?? "N/A";
            PriceDisplay.Text = $"${Property.Price:N0}";
            StatusText.Text = Property.Status ?? "Unknown";
            PropertyNumberDisplay.Text = Property.Code ?? "N/A";

            // Status badge coloring
            StatusBadgeContainer.Background = Property.Status switch
            {
                "On Sale" => new SolidColorBrush(Colors.Green),
                "Occupied" => new SolidColorBrush(Colors.Orange),
                "Vacant" => new SolidColorBrush(Colors.Blue),
                "Under Maintenance" => new SolidColorBrush(Colors.Purple),
                "Pending" => new SolidColorBrush(Colors.Yellow),
                "Sold" => new SolidColorBrush(Colors.Red),
                _ => new SolidColorBrush(Colors.Gray)
            };

            // Load property image with fallback
            try
            {
                if (!string.IsNullOrEmpty(Property.ImagePath))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Property.ImagePath, UriKind.RelativeOrAbsolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    PropertyImage.Source = bitmap;
                }
                else
                {
                    SetDefaultImage();
                }
            }
            catch
            {
                SetDefaultImage();
            }
        }

        private void SetDefaultImage()
        {
            try
            {
                PropertyImage.Source = new BitmapImage(new Uri("Images/default.jpg", UriKind.Relative));
            }
            catch
            {
                // In case the fallback image is missing
                PropertyImage.Source = null;
            }
        }



        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
