using System;
using System.Globalization;
using System.Windows.Data;

namespace Real_Estate_Agencies
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d) return d.ToString("C0");
            if (value is double db) return db.ToString("C0");
            if (value is int i) return i.ToString("C0");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
