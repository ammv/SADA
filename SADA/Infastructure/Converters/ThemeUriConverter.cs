using HandyControl.Themes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SADA.Infastructure.Converters
{
    class ThemeUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string uri = (string)parameter;
            if (ThemeManager.Current.ApplicationTheme == ApplicationTheme.Light)
            {
                uri = uri.Replace("dark", "light");
            }
            else
            {
                uri = uri.Replace("light", "dark");
            }

            return new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
