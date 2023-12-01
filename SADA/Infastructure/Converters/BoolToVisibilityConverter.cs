using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? inverseEnabled = (bool?)parameter;
            bool val = (bool)value;
            if(inverseEnabled == true)
            {
                val = !val;
            }
            if (val == true)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
    }
}