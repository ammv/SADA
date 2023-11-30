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
    class AddressToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is Address address))
            {
                return Binding.DoNothing;
            }
            List<string> data = new List<string>();
            AddStringToList(data, address.PostalIndex, ", ");
            AddStringToList(data, address.Street?.District?.City?.Region?.Country?.Name, ", ");
            AddStringToList(data, address.Street?.District?.City?.Region?.Name, ", ");
            AddStringToList(data, address.Street?.District?.City?.Name, ", ", "г. ");
            AddStringToList(data, address.Street?.District?.Name, ", ");
            AddStringToList(data, address.Street?.Name, ", ");
            AddStringToList(data, address.BuildingNumber, ", ", "д. ");
            AddStringToList(data, address.Corpus, ", ", "к. ");
            AddStringToList(data, address.Floor, ", ", "эт. ");
            AddStringToList(data, address.Flat, null, "кв. ");

            return string.Join(string.Empty, data);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        private void AddStringToList(List<string> list, object s, string postfix = null, string prefix = null)
        {
            if (string.IsNullOrEmpty(s?.ToString())) return;
            list.Add($"{prefix}{s}{postfix}");
        }
    }
}
