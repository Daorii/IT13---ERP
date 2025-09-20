using System;
using System.Globalization;
using System.Windows.Data;

namespace Real_Estate_Agencies.Converters
{
    public class StaggerDelayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
                return TimeSpan.FromSeconds(index * 0.1);
            return TimeSpan.Zero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
