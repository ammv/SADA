using SADA.Helpers;
using SADA.Infastructure.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    class ComparisonOperatorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ComparisonOperator comparisonOperator)
            {
                if(ComparisonOperatorHelper.ComparisonOperatorMap.TryGetValue(comparisonOperator, out string result))
                {
                    return result;
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (ComparisonOperatorHelper.ComparisonOperatorReverseMap.TryGetValue(s, out ComparisonOperator result))
                {
                    return result;
                }
            }
            return Binding.DoNothing;
        }
    }
}
