using CommunityToolkit.Mvvm.Input;
using SADA.Infastructure.Core.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Core
{
    public abstract class TabObservableObjectList<T>: TabObservableObjectWithLoading
        where T: class, new()
    {

        #region Fields

        protected int _dataCountPerPage = 20;
        protected int _maxPage = 0;
        protected ListMode _currentListMode = ListMode.Default;
        protected Action<T> _selectAction;
        protected ObservableCollection<T> _entities;
        protected T _selectedEntity = null;

        /// <summary>
        /// Добавляется ко всем базовым запросам
        /// </summary>
        protected Expression<Func<T, bool>> _baseFilter;

        /// <summary>
        /// Текущий запрос для пагинации
        /// </summary>
        protected IQueryable<T> _currentQuery;

        /// <summary>
        /// Базовый запрос по умолчанию, когда нет данных
        /// </summary>
        protected IQueryable<T> _defaultQuery;

        private AsyncRelayCommand<HandyControl.Data.FunctionEventArgs<int>> _pageUpdateCommand;

        //protected IQueryable<T> _defaultQuery { get; }
        //protected IQueryable<T> _currentQuery { get; }

        #endregion

        #region Constructor
        #endregion

        #region Properties

        public Action<T> SelectAction
        {
            get => _selectAction;
            set => SetProperty(ref _selectAction, value);
        }
        public virtual ObservableCollection<T> Entities
        {
            get { return _entities; }
            set 
            { 
                if(SetProperty(ref _entities, value))
                {
                    MaxPage = _entities.Count() / _dataCountPerPage + 1;
                }
            }
        }

        public virtual int DataCountPerPage
        {
            get { return _dataCountPerPage; }
            set
            {
                if (SetProperty(ref _dataCountPerPage, value))
                {
                    MaxPage = _entities.Count() / _dataCountPerPage + 1;
                }
            }
        }

        public virtual int MaxPage
        {
            get { return _maxPage; }
            set { SetProperty(ref _maxPage, value); }
        }

        public virtual ListMode CurrentListMode
        {
            get { return _currentListMode; }
            set { SetProperty(ref _currentListMode, value); }
        }

        public virtual T SelectedEntity
        {
            get { return _selectedEntity; }
            set { SetProperty(ref _selectedEntity, value); }
        }
        #endregion

        #region Commands
        public AsyncRelayCommand<HandyControl.Data.FunctionEventArgs<int>> PageUpdateCommand
        {
            get
            {
                if(_pageUpdateCommand == null)
                {
                    SetProperty(ref _pageUpdateCommand, new AsyncRelayCommand<HandyControl.Data.FunctionEventArgs<int>>(_PageUpdateCommand));
                }
                return _pageUpdateCommand;
            }
            protected set
            {
                SetProperty(ref _pageUpdateCommand, value);
            }
        }

        #endregion

        #region Command implementation

        protected abstract Task _PageUpdateCommand(HandyControl.Data.FunctionEventArgs<int> e);

        #endregion

        #region Other
        #endregion

    }
}
