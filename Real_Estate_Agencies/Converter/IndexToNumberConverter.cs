using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Real_Estate_Agencies.Converters
{
    public class IndexToNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable items && parameter is object item)
            {
                int index = 0;
                foreach (var i in items)
                {
                    if (i == item)
                        return (index + 1).ToString();
                    index++;
                }
            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
