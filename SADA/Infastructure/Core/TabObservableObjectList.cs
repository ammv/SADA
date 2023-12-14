using CommunityToolkit.Mvvm.Input;
using SADA.Helpers;
using SADA.Infastructure.Core.Enums;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.Infastructure.Core
{
    public abstract class TabObservableObjectList<T>: TabObservableObjectWithLoading
        where T: class, new()
    {

        #region Fields

        private Dictionary<ListMode, string> _listModeMap;

        protected int _pageIndex = 1;
        protected int _dataCountPerPage = 20;
        protected int _maxPage = 0;
        protected ListMode _currentListMode = ListMode.Default;
        protected Action<T> _selectAction;
        protected ObservableCollection<T> _entities;
        protected T _selectedEntity = null;

        protected readonly IDialogService _dialogService;
        protected readonly ITabService _tabService;

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
        private RelayCommand<FormMode> _openEntityFormCommand;

        private RelayCommand _searchCommand;
        private RelayCommand _saveAsFileCommand;
        private RelayCommand _applyFilterCommand;
        private RelayCommand _clearFilterCommand;

        private readonly TypeWrapper<TabObservableObjectForm<T>> _formType;

        #endregion

        #region Constructor

        protected TabObservableObjectList(IDialogService dialogService, ITabService tabService, TypeWrapper<TabObservableObjectForm<T>> formType)
        {
            _dialogService = dialogService;
            _tabService = tabService;
            _formType = formType;
            
        }

        #endregion

        #region Properties

        public Dictionary<ListMode, string> ListModeMap { get; set; }

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

        public virtual int PageIndex
        {
            get { return _pageIndex; }
            set { SetProperty(ref _pageIndex, value); }
        }

        public virtual ListMode CurrentListMode
        {
            get { return _currentListMode; }
            set
            {
                if(SetProperty(ref _currentListMode, value))
                {
                    if (_listModeMap == null) return;
                    if(_listModeMap.TryGetValue(value, out string s))
                    {
                        Name = s;
                    }
                }
            }
        }

        public virtual T SelectedEntity
        {
            get { return _selectedEntity; }
            set { SetProperty(ref _selectedEntity, value); }
        }

        public Func<T, string> AddTabName { get; set; }
        public Func<T, string> EditTabName { get; set; }

        #endregion

        #region Commands
        public AsyncRelayCommand<HandyControl.Data.FunctionEventArgs<int>> PageUpdateCommand
        {
            get
            {
                return _pageUpdateCommand = _pageUpdateCommand ??
                    new AsyncRelayCommand<HandyControl.Data.FunctionEventArgs<int>>(_PageUpdateCommand);
            }
        }

        public RelayCommand<FormMode> OpenEntityFormCommand
        {
            get
            {
                return _openEntityFormCommand = _openEntityFormCommand ??
                    new RelayCommand<FormMode>(_OpenEntityFormCommand);
            }
        }

        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand = _searchCommand ??
                    new RelayCommand(_SearchCommand);
            }
        }

        public RelayCommand ApplyFilterCommand
        {
            get
            {
                return _applyFilterCommand = _applyFilterCommand ??
                    new RelayCommand(_ApplyFilterCommand);
            }
        }

        public RelayCommand ClearFilterCommand
        {
            get
            {
                return _clearFilterCommand = _clearFilterCommand ??
                    new RelayCommand(_ClearFilterCommand);
            }
        }

        public RelayCommand SaveAsFileCommand
        {
            get
            {
                return _saveAsFileCommand = _saveAsFileCommand ??
                    new RelayCommand(_SaveAsFileCommand);
            }
        }

        #endregion

        #region Command implementation

        private async Task _PageUpdateCommand(HandyControl.Data.FunctionEventArgs<int> e)
        {
            try
            {
                MaxPage = _currentQuery.Count() / _dataCountPerPage;
                Entities = new ObservableCollection<T>(
                    await JoinBaseQuery(_currentQuery)
                    .Skip((e.Info - 1) * _dataCountPerPage)
                    .Take(_dataCountPerPage)
                    .ToListAsync());
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        /// <summary>
        /// Реализация команды ответственной за открытие формы ссущности в различных режимах
        /// </summary>
        /// <param name="formMode"></param>
        private void _OpenEntityFormCommand(FormMode formMode)
        {
            if (_currentListMode == ListMode.Select && _selectedEntity != null)
            {
                _selectAction?.Invoke(_selectedEntity);
                _RaiseCloseEvent();

            }
            else
            {
                if (formMode == FormMode.Edit && _selectedEntity == null) return;

                var vm = App.Current.Services.GetService(_formType.TypeDerived) as TabObservableObjectForm<T>;
                vm.Configure(formMode, _selectedEntity);
                _tabService.OpenTab(vm);
            }
        }

        protected abstract void _SearchCommand();
        protected abstract void _ApplyFilterCommand();
        protected abstract void _ClearFilterCommand();
        protected abstract void _SaveAsFileCommand();


        #endregion

        #region Other

        protected bool Select()
        {
            if (_currentListMode == ListMode.Select && _selectedEntity != null)
            {
                _selectAction?.Invoke(_selectedEntity);
                _RaiseCloseEvent();
                return true;

            }

            return false;
        }

        protected abstract IQueryable<T> JoinBaseQuery(IQueryable<T> query);

        

        #endregion

    }
}
