using System;
using System.Globalization;
using System.Windows.Data;
using StatCardApp.Model.CardFields;

namespace StatCardApp.Convertors
{
    public class BirthPlaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Place birthPlace = (Place)value;

            return $"{birthPlace.Region}, {birthPlace.Area}, {birthPlace.Locality}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}