using System;
using System.Globalization;
using System.Windows.Data;

namespace Real_Estate_Agencies
{
    public class StaggerDelayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;
            return TimeSpan.FromMilliseconds(index * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
