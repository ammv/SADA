using HandyControl.Themes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    class ThemeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(ThemeManager.Current.ApplicationTheme == ApplicationTheme.Light)
            {
                return MahApps.Metro.IconPacks.PackIconBootstrapIconsKind.Moon;
            }
            else
            {
                return MahApps.Metro.IconPacks.PackIconBootstrapIconsKind.Sun;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
