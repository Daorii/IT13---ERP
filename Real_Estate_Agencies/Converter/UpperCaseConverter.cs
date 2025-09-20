using System;
using System.Globalization;
using System.Windows.Data;

namespace Real_Estate_Agencies
{
    public class UpperCaseConverter : IValueConverter
    {
        public static UpperCaseConverter Instance { get; } = new UpperCaseConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return str.ToUpper();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
