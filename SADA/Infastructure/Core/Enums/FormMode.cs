using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Core.Enums
{
    public enum FormMode
    {
        /// <summary>
        /// Режим не задан
        /// </summary>
        NotSet,
        /// <summary>
        /// Форма открыта в режиме добавления новой сущности
        /// </summary>
        Add,
        /// <summary>
        /// Форма открыта в режиме редактирования сущности
        /// </summary>
        Edit,
        /// <summary>
        /// Форма открыта только для просмотра сущности
        /// </summary>
        See
    }
}
