using System;
using System.Globalization;
using System.Windows.Data;
using StatCardApp.Function;

namespace StatCardApp.Convertors
{
    public class ValueToSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string strValue = StatCardFunc.ConvertSizeValue((int)value, System.Convert.ToInt32(parameter));
                return strValue;
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