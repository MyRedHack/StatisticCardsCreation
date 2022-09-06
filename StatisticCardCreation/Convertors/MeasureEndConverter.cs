using System;
using System.Globalization;
using System.Windows.Data;
using StatCardApp.Model.CardFields;

namespace StatCardApp.Convertors
{
    public class MeasureEndConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? MeasureDuration = (DateTime?)value;

            if (MeasureDuration.HasValue) return $"до {MeasureDuration.Value.ToShortDateString()}";
            else return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
