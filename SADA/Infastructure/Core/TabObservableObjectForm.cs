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
    abstract class TabObservableObjectForm<T>: TabObservableObjectWithLoading
        where T: class
    {
        protected FormMode _formMode = FormMode.Add;
        protected T _entity = null;
        
        public FormMode FormMode
        {
            get => _formMode;
            set => SetProperty(ref _formMode, value);
        }

        public T Entity
        {
            get => _entity;
            set => SetProperty(ref _entity, value);
        }
    }
}
