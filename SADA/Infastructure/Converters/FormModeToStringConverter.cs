using SADA.Infastructure.Core;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    internal class FormModeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FormMode formMode = (FormMode)value;

            switch (formMode)
            {
                case FormMode.Add:
                    return "Создать";

                case FormMode.Edit:
                    return "Изменить";

                default:
                    return "Неизвестная операция";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}