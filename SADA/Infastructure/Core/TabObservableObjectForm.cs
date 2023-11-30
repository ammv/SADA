using SADA.Infastructure.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Core
{
    /// <summary>
    /// Представляет вкладку представляющию форму для работы с конкретной сущностью
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TabObservableObjectForm<T>: TabObservableObjectWithLoading
        where T: class
    {
        protected FormMode _currentFormMode = FormMode.NotSet;
        protected T _entity = null;
        
        public virtual FormMode CurrentFormMode
        {
            get => _currentFormMode;
            set => SetProperty(ref _currentFormMode, value);
        }

        public T Entity
        {
            get => _entity;
            set => SetProperty(ref _entity, value);
        }
    }
}
