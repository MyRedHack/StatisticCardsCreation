using System;
using System.Globalization;
using System.Windows.Data;

namespace StatCardApp.Convertors
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strTime = String.Empty;

            strTime = ((TimeSpan)value).ToString();
            strTime = strTime.Substring(0, 5);
            return strTime;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}