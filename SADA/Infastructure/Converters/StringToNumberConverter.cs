using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    class StringToNumberConverter : IValueConverter
    {
        /// <summary>
        /// Преобразований из числа в строку не требуется
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (value as string).Replace('.', ',');

            Type underlyingType = Nullable.GetUnderlyingType(targetType);
            if(underlyingType != null)
            {
                targetType = underlyingType;
            }

            return TypeDescriptor.GetConverter(targetType).ConvertFromInvariantString(s);
        }
    }
}
