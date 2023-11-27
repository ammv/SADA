using DataLayer;
using DataLayer.Helpers;
using SADA.Helpers;
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
            var counteragent = value as Counteragent;
            if (counteragent == null) return null;
            string text;

            switch(CounteragentTypeHelper.CounteragentTypeMap[counteragent.CounteragentType.Name])
            {
                case CounteragentTypeEnum.IndividualPerson:
                    IndividualPerson individualPerson = counteragent.IndividualPerson;
                    text = FullNameToInitialsHelper.MakeInitials(individualPerson.Name, individualPerson.Surname, individualPerson.Patronymic);
                    break;

                case CounteragentTypeEnum.JuridicalPerson:
                    JuridicalPerson juridicalPerson = counteragent.JuridicalPerson;
                    text =  juridicalPerson.ShortName;
                    break;

                case CounteragentTypeEnum.SoloTrader:
                    SoloTrader soloTrader = counteragent.SoloTrader;
                    text = soloTrader.ShortName;
                    break;

                default:
                    return "Неизвестно";
            }

            return $"{text} ({counteragent.CounteragentGroup.Name})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
