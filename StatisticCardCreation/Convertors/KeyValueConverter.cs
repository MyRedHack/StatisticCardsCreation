using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace StatCardApp.Convertors
{
    public class KeyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                PropertyInfo prop = typeof(ListsData).GetProperty((string)parameter);
                List<El> list = (List<El>)prop.GetValue(null);
                string strFieldValue = (from el in list where el.Num == System.Convert.ToInt32(value) select el.Str).Single();
                return strFieldValue;
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