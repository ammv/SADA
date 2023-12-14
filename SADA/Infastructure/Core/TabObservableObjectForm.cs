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
        private Dictionary<FormMode, Func<T, string>> _formModeTabNameMap;
        private Dictionary<FormMode, Action> _formModeActionMap;

        protected FormMode _currentFormMode = FormMode.NotSet;
        protected T _entity = null;

        public Dictionary<FormMode, Func<T, string>> FormModeTabNameMap 
        { 
            get => _formModeTabNameMap;
            set => SetProperty(ref _formModeTabNameMap, value);
        }
        public Dictionary<FormMode, Action> FormModeActionMap
        {
            get => _formModeActionMap;
            set => SetProperty(ref _formModeActionMap, value);
        }

        public FormMode CurrentFormMode
        {
            get => _currentFormMode;
            set
            { 
                if(SetProperty(ref _currentFormMode, value))
                {
                    FormModeTabNameUpdate();
                    FormModeActionInvoke();
                }
            } 
        }

        public T Entity
        {
            get => _entity;
            set => SetProperty(ref _entity, value);
        }

        public void Configure(FormMode formMode, T entity = null)
        {
            if(FormMode.Edit == formMode || FormMode.See == formMode) Entity = entity;
            CurrentFormMode = formMode;
        }

        public void FormModeTabNameUpdate()
        {
            if (_formModeTabNameMap == null) return;
            if (_formModeTabNameMap.TryGetValue(_currentFormMode, out var func))
            {
                Name = func?.Invoke(_entity);
            }
        }

        public void FormModeActionInvoke()
        {
            if (_formModeActionMap == null) return;
            if (_formModeActionMap.TryGetValue(_currentFormMode, out var action))
            {
                action?.Invoke();
            }
        }
    }
}
