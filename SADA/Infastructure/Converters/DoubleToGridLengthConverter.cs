using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    public class DoubleToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double length)
            {
                return new GridLength(length);
            }
            return new GridLength();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GridLength length)
            {
                return length.Value;
            }
            return double.NaN;
        }
    }
}