using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Real_Estate_Agencies.Converter
{
    public class Base64ToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string base64String = value as string;
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                // Remove "data:image/png;base64," prefix if it exists
                if (base64String.Contains(","))
                    base64String = base64String.Substring(base64String.IndexOf(",") + 1);

                byte[] imageBytes = System.Convert.FromBase64String(base64String);
                using (var ms = new MemoryStream(imageBytes))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
