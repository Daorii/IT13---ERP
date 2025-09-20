using System;
using System.Globalization;
using System.Windows.Data;

namespace Real_Estate_Agencies
{
    public class StaggerDelayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                // Create a staggered delay based on the index
                return TimeSpan.FromMilliseconds(index * 100); // 100ms delay per item
            }
            return TimeSpan.Zero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
