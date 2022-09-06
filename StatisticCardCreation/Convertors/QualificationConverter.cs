using System;
using System.Globalization;
using System.Windows.Data;
using StatCardApp.Model.CardFields;

namespace StatCardApp.Convertors
{
    public class QualificationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CrimeQualification qualification = (CrimeQualification)value;
            string result = string.Empty;

            if (qualification.Article.HasValue)
            {
                result += $"Ст. {qualification.Article}";
            }

            if (!string.IsNullOrEmpty(qualification.Sign))
            {
                result += "." + qualification.Sign;
            }

            if (!string.IsNullOrWhiteSpace(qualification.Part))
            {
                result += $" ч. {qualification.Part} ";
            }

            if (!string.IsNullOrWhiteSpace(qualification.Paragraph))
            {
                result += $" п. «{qualification.Paragraph}»";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
