using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    class CarToStringInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataLayer.Car car = value as DataLayer.Car;
            if (car == null) return Binding.DoNothing;

            string text = $"{car.CarEquipment?.CarModel?.CarBrand?.Name} {car.CarEquipment?.CarModel?.Name} {car.CarEquipment?.Name}";

            if(parameter is bool isExtend && isExtend)
            {
                text = $"{text} {car.YearOfRelease}";
            }

            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
