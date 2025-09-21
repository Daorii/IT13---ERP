using System;
using System.Globalization;
using System.Windows.Data;

namespace Real_Estate_Agencies
{
    public class StaggerDelayConverter : IValueConverter
    {
        // Converts an index into a TimeSpan delay
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                // Each item gets a delay of 100ms * index
                return TimeSpan.FromMilliseconds(index * 100);
            }
            return TimeSpan.Zero;
        }

        // Not used, but required by IValueConverter
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
