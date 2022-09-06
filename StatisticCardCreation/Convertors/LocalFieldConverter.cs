using System;
using System.Globalization;
using System.Windows.Data;

namespace StatCardApp.Convertors
{
    public class LocalFieldConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((Boolean?)value).Value)
            {
                return "50";
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}