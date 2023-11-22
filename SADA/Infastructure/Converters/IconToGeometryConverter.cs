using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace SADA.Infastructure.Converters
{
    class IconToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string icon)
            {
                return Geometry.Parse(new PackIconBootstrapIcons
                {
                    Kind = (PackIconBootstrapIconsKind)Enum.Parse(typeof(PackIconBootstrapIconsKind), icon)
                }.Data);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
