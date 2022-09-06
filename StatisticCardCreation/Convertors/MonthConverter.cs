using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace StatCardApp.Convertors
{
    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Dictionary<int, string> months = new Dictionary<int, string>()
            {
                { 1, "Январь" },
                { 2, "Февраль" },
                { 3, "Март" },
                { 4, "Апрель" },
                { 5, "Май" },
                { 6, "Июнь" },
                { 7, "Июль" },
                { 8, "Август" },
                { 9, "Сентябрь" },
                { 10, "Октябрь" },
                { 11, "Ноябрь" },
                { 12, "Декабрь" },
            };

            if ((int?)value != null)
            {
                return months[(int)value];
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