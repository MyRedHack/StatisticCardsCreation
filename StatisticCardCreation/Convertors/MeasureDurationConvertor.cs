using System;
using System.Globalization;
using System.Windows.Data;
using StatCardApp.Model.CardFields;

namespace StatCardApp.Convertors
{
    public class MeasureDurationConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? MeasureDuration = (int?)value;

            if (MeasureDuration.HasValue) return $"{MeasureDuration} месяцев";
            else return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}