using System;
using System.Globalization;
using System.Windows.Data;

namespace StatCardApp.Convertors
{
    public class NumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "" || value.ToString().Contains(" "))
            {
                return null;
            }
            else
            {
                return System.Convert.ToInt64(value);
            }
        }
    }
}