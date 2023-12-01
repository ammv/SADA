using DataLayer;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SADA.Infastructure.Converters
{
    public class StaffToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Staff staff)
            {
                if (staff.Passport == null)
                {
                    return "Неизвестно";
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(staff.Passport.Surname);
                sb.Append(" ");
                sb.Append(staff.Passport.Name);
                sb.Append(" ");
                if (!string.IsNullOrEmpty(staff.Passport.Patronymic))
                {
                    sb.Append(staff.Passport.Patronymic);
                    sb.Append(" ");
                }
                if (staff.StaffPost == null)
                {
                    sb.Append($"(Неизвестно)");
                }
                else
                {
                    sb.Append($"({staff.StaffPost.Name})");
                }

                return sb.ToString();
            }
            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}