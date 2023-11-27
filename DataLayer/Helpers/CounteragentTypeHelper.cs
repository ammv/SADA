using System.Collections.Generic;

namespace DataLayer.Helpers
{
    /// <summary>
    /// Перечисление типов организаций
    /// </summary>
    public enum CounteragentTypeEnum
    {
        /// <summary>
        /// Физическое лицо
        /// </summary>
        IndividualPerson,

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        JuridicalPerson,

        /// <summary>
        /// Индивидуальный предприниматель
        /// </summary>
        SoloTrader,
    }

    public class CounteragentTypeHelper
    {


        public static Dictionary<string, CounteragentTypeEnum> CounteragentTypeMap = new Dictionary<string, CounteragentTypeEnum>
        {
            {"Физическое лицо", CounteragentTypeEnum.IndividualPerson },
            {"Юридическое лицо", CounteragentTypeEnum.JuridicalPerson },
            {"Индивидуальный предприниматель", CounteragentTypeEnum.SoloTrader },
        };



    }
}