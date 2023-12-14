using CommunityToolkit.Mvvm.Input;
using DataLayer;
using SADA.Helpers;
using SADA.Infastructure.Converters;
using SADA.Infastructure.Core;
using SADA.Infastructure.Core.Enums;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.MainMenu.Home.Counteragent
{
    public class CounteragentListViewModel : TabObservableObjectList<DataLayer.Counteragent>
    {
        #region Fields

        #region Services fields

        #endregion Services fields

        #region IEnumerables fields

        private IEnumerable<CounteragentType> _counteragentTypes;
        private IEnumerable<CounteragentGroup> _counteragentGroups;

        #endregion IEnumerables fields

        #region Selected fields

        private CounteragentType _selectedCounteragentType;
        private CounteragentGroup _selectedCounteragentGroup;

        #endregion Selected fields

        #region Other fields

        private SADAEntities _ctx;

        private CounteragentToStringInfoConverter _counteragentToStringInfoConverter = new CounteragentToStringInfoConverter();

        #endregion Other fields

        #region Filter fields

        private FilterMaker _filter;

        #endregion Filter fields

        #endregion Fields

        #region Constructor

        public CounteragentListViewModel(IDialogService dialogService, IWindowService windowService, ITabService tabService):
            base(dialogService, tabService, TypeWrapper<TabObservableObjectForm<DataLayer.Counteragent>>.Make<CounteragentViewModel>())
        {
            //EditTabName = (e) =>
            //{
            //    string info = _counteragentToStringInfoConverter.Convert(SelectedEntity, null, null, null).ToString();
            //    return $"Изменение контрагента {info} №{_selectedEntity.ID}";
            //};

            //AddTabName = (e) => "Добавление нового контрагента";

            _filter = new FilterMaker(_tabService);

            ListModeMap = new Dictionary<ListMode, string>
            {
                {ListMode.Default, "Контрагенты" },
                {ListMode.Select, "Выбор контрагента" },
            };
        }

        protected CounteragentListViewModel(): base(null, null, null)
        { }


        #endregion Constructor

        #region Properties

        public IEnumerable<CounteragentGroup> CounteragentGroups
        {
            get => _counteragentGroups;
            set => SetProperty(ref _counteragentGroups, value);
        }

        public IEnumerable<CounteragentType> CounteragentTypes
        {
            get => _counteragentTypes;
            set => SetProperty(ref _counteragentTypes, value);
        }


        public CounteragentGroup SelectedCounteragentGroup
        {
            get => _selectedCounteragentGroup;
            set => SetProperty(ref _selectedCounteragentGroup, value);
        }

        public CounteragentType SelectedCounteragentType
        {
            get => _selectedCounteragentType;
            set => SetProperty(ref _selectedCounteragentType, value);
        }

        #region Filter properties

        public FilterMaker Filter
        {
            get => _filter;
        }

        #endregion Filter properties

        #endregion Properties

        #region Commands
        public RelayCommand<Type> OpenTypeListCommand { get; }

        #endregion Commands

        #region Command implementation

        protected override void _OnClose()
        {
            var result = _dialogService.ShowMessageBox("Вопрос", $"Закрыть вкладку {Name}?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _RaiseCloseEvent();
                _ctx?.Dispose();
            }
        }

        protected override void _SearchCommand()
        {
            // Базовый поиск по типу оплаты, контрагенту

            _currentQuery = _defaultQuery;

            if (_selectedCounteragentGroup != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.GroupID == _selectedCounteragentGroup.ID);
            }

            if (_selectedCounteragentType != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.TypeID == _selectedCounteragentType.ID);
            }

            try
            {
                Entities = new ObservableCollection<DataLayer.Counteragent>(
                _currentQuery
                .Where(_baseFilter)
                .Take(_dataCountPerPage)
                .ToList());
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox("Ошибка", ex.Message, MessageBoxButton.OK);
            }
        }

        protected override void _SaveAsFileCommand()
        {
        }

        protected override void _ApplyFilterCommand()
        {
            try
            {
                _currentQuery = _defaultQuery.Where(_filter.MakeFilter());
                Entities = new ObservableCollection<DataLayer.Counteragent>(
                    _currentQuery.Take(_dataCountPerPage).ToList());
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        protected override void _ClearFilterCommand()
        {
            _filter.FilterFieldsClear();
        }

        protected override void LoadedInner()
        {
            try
            {
                _ctx = new SADAEntities();

                _baseFilter = c => c.IsDeleted == false;

                _defaultQuery = JoinBaseQuery(_ctx.Counteragent);

                Entities = new ObservableCollection<DataLayer.Counteragent>
                    (_defaultQuery.Take(_dataCountPerPage).ToList());

                CounteragentGroups = _ctx.CounteragentGroup
                    .AsNoTracking()
                    .ToList();

                CounteragentTypes = _ctx.CounteragentType
                    .AsNoTracking()
                    .ToList();

                _filter.CounteragentGroups = _counteragentGroups;
                _filter.CounteragentTypes = _counteragentTypes;
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        #endregion Command implementation

        #region Other

        protected override IQueryable<DataLayer.Counteragent> JoinBaseQuery(IQueryable<DataLayer.Counteragent> query)
        {
            return query
                .Include(c => c.CounteragentGroup)
                .Include(c => c.CounteragentType)
                .Include(c => c.CarDealership)
                .AsNoTracking();
        }

        public sealed class FilterMaker : EntityFilterBase<DataLayer.Counteragent>
        {
            #region Filter private fields

            private IEnumerable<CounteragentType> _counteragentTypes;
            private IEnumerable<CounteragentGroup> _counteragentGroups;

            private CounteragentType _selectedCounteragentType;
            private CounteragentGroup _selectedCounteragentGroup;

            #endregion Filter private fields

            #region Filter properties

            public IEnumerable<CounteragentGroup> CounteragentGroups
            {
                get => _counteragentGroups;
                set => SetProperty(ref _counteragentGroups, value);
            }

            public IEnumerable<CounteragentType> CounteragentTypes
            {
                get => _counteragentTypes;
                set => SetProperty(ref _counteragentTypes, value);
            }


            public CounteragentGroup SelectedCounteragentGroup
            {
                get => _selectedCounteragentGroup;
                set => SetProperty(ref _selectedCounteragentGroup, value);
            }

            public CounteragentType SelectedCounteragentType
            {
                get => _selectedCounteragentType;
                set => SetProperty(ref _selectedCounteragentType, value);
            }

            public bool ShowIsDeleted { get; set; } = false;

            #endregion Filter properties

            public override Expression<Func<DataLayer.Counteragent, bool>> MakeFilter()
            {
                var expression = defaultExpression;

                if (SelectedCounteragentGroup != null)
                {
                    expression = expression
                        .And(c => c.GroupID == SelectedCounteragentGroup.ID);
                }

                if (SelectedCounteragentType != null)
                {
                    expression = expression
                        .And(c => c.TypeID == SelectedCounteragentType.ID);
                }


                if (ShowIsDeleted == true)
                {
                    expression = expression.And(c => c.IsDeleted == true || c.IsDeleted == false);
                }


                return expression;
            }

            private ITabService _tabService;

            public FilterMaker(ITabService tabService)
            {
                OpenTypeListCommand = new RelayCommand<Type>(_OpenTypeListCommand);

                _tabService = tabService;
            }

            public RelayCommand<Type> OpenTypeListCommand { get; }


            private void _OpenTypeListCommand(Type type)
            {
                switch (type.Name)
                {
                    case nameof(CounteragentGroup):
                        break;
                    case nameof(CounteragentType):
                        break;
                }
            }

        }

        #endregion Other
    }
}