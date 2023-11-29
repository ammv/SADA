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
            if (counteragent == null) return Binding.DoNothing;
            string text;

            switch(CounteragentTypeHelper.CounteragentTypeMap[counteragent.CounteragentType.Name])
            {
                case CounteragentTypeEnum.IndividualPerson:
                    IndividualPerson individualPerson = counteragent.IndividualPerson;
                    if (individualPerson == null) return Binding.DoNothing;
                    text = FullNameToInitialsHelper.MakeInitials(individualPerson.Name, individualPerson.Surname, individualPerson.Patronymic);
                    break;

                case CounteragentTypeEnum.JuridicalPerson:
                    JuridicalPerson juridicalPerson = counteragent.JuridicalPerson;
                    if (juridicalPerson == null) return Binding.DoNothing;
                    text =  juridicalPerson.ShortName;
                    break;

                case CounteragentTypeEnum.SoloTrader:
                    SoloTrader soloTrader = counteragent.SoloTrader;
                    if (soloTrader == null) return Binding.DoNothing;
                    text = soloTrader.ShortName;
                    break;

                default:
                    return "Неизвестный тип контрагента";
            }

            return $"{text} ({counteragent.CounteragentGroup?.Name})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
