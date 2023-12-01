using System;
using System.Globalization;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    public class LongToShortStringConverter : IValueConverter
    {
        public int? MaxLength { get; set; } = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                int maxLength = MaxLength ?? (int)parameter;
                if (s.Length > maxLength)
                {
                    return $"{s.Substring(0, maxLength)}...";
                }
                return s;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}