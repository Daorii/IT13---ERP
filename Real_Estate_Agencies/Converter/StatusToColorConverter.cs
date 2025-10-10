using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Real_Estate_Agencies.Converter
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value as string;

            return status switch
            {
                "Available" => new SolidColorBrush(Color.FromRgb(34, 197, 94)), // Green
                "Sold" => new SolidColorBrush(Color.FromRgb(239, 68, 68)),      // Red
                "Pending" => new SolidColorBrush(Color.FromRgb(250, 204, 21)),  // Yellow
                _ => new SolidColorBrush(Color.FromRgb(107, 114, 128))          // Gray
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
