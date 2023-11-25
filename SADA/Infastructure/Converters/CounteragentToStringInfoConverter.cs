using DataLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    class CounteragentToStringInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var counteragent = value as Counteragent;
            //if(counteragent.IndividualPerson.Count != 0)
            //{
            //    var individualPerson = counteragent.IndividualPerson.First();
            //}
            //else if(counteragent.JuridicalPerson.Count != 0)
            //{

            //}
            //else if()
            //{

            //}
            //else
            //{
            //    return string.Empty;
            //}
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
