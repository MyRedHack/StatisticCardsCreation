using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace StatCardApp.Convertors
{
    public class LivePlaceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string result = String.Empty;

            for (int i = 0; i < values.Count(); i++)
            {
                if (values[i] != null)
                {
                    if (i != values.Count() - 1)
                    {
                        result += $"{values[i]}, ";
                    }
                    else
                    {
                        result += $"{values[i]}";
                    }
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}