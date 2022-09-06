using System;
using System.Globalization;
using System.Windows.Data;

namespace StatCardApp.Convertors
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strDate = String.Empty;
            try
            {
                if ((string)parameter == "ShortYear")
                {
                    strDate = ((DateTime?)value).Value.ToShortDateString().Remove(6, 2);
                }
                else
                {
                    strDate = ((DateTime?)value).Value.ToShortDateString();
                }
            }
            catch { }

            return strDate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}