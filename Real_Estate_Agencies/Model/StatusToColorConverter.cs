using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Real_Estate_Agencies
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status.ToLower())
                {
                    case "on sale":
                    case "available":
                        return Brushes.Green;
                    case "sold":
                        return Brushes.Red;
                    default:
                        return Brushes.Gold;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
