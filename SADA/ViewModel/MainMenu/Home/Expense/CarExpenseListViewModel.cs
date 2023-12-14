using CommunityToolkit.Mvvm.Input;
using DataLayer;
using SADA.Helpers;
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

namespace SADA.ViewModel.MainMenu.Home.Expense
{
    public class CarExpenseListViewModel : TabObservableObjectList<CarExpense>
    {
        #region Fields

        #region Services fields

        #endregion Services fields

        #region IEnumerables fields

        private IEnumerable<ExpenseGroup> _expenseGroups;
        private ObservableCollection<DataLayer.Car> _cars;

        #endregion IEnumerables fields

        #region Selected fields

        private ExpenseType _selectedExpenseType;
        private ExpenseGroup _selectedExpenseGroup;
        private DataLayer.Car _selectedCar;

        #endregion Selected fields

        #region Other fields

        private SADAEntities _ctx;

        #endregion Other fields

        #region Filter fields

        private FilterMaker _filter;

        #endregion Filter fields

        #endregion Fields

        #region Constructor

        public CarExpenseListViewModel(IDialogService dialogService, ITabService tabService)
            :base(dialogService, tabService, TypeWrapper<TabObservableObjectForm<CarExpense>>.Make<CarExpenseViewModel>())
        {
            _filter = new FilterMaker(_tabService);

            //AddTabName = (e) => "Добавление расхода на автомобиль";
            //EditTabName = (e) => $"Изменение расхода на автомобиль №{_selectedEntity.ID}";

            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

            ListModeMap = new Dictionary<ListMode, string>
            {
                {ListMode.Default, "Расходы на автомобили" },
                {ListMode.Select, "Выбор расхода на автомобиль" },
            };
        }

        protected CarExpenseListViewModel(): base(null, null, null)
        { }


        #endregion Constructor

        #region Properties

        public IEnumerable<ExpenseGroup> ExpenseGroups
        {
            get => _expenseGroups;
            set => SetProperty(ref _expenseGroups, value);
        }

        public ObservableCollection<DataLayer.Car> Cars
        {
            get => _cars;
            set => SetProperty(ref _cars, value);
        }

        public ExpenseType SelectedExpenseType
        {
            get => _selectedExpenseType;
            set => SetProperty(ref _selectedExpenseType, value);
        }

        public ExpenseGroup SelectedExpenseGroup
        {
            get => _selectedExpenseGroup;
            set => SetProperty(ref _selectedExpenseGroup, value);
        }

        public DataLayer.Car SelectedCar
        {
            get => _selectedCar;
            set => SetProperty(ref _selectedCar, value);
        }


        #region Filter properties

        public FilterMaker Filter
        {
            get => _filter;
        }

        #endregion Filter properties

        #endregion Properties

        #region Commands
        public AsyncRelayCommand<Type> OpenTypeListCommand { get; }

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

        private async Task _OpenTypeListCommand(Type type)
        {
            switch (type.Name)
            {
                case nameof(DataLayer.Car):
                    _tabService.OpenTabForSelect<Car.Salon.CarInSalonListViewModel, DataLayer.Car>(
                        "Выбор автомобиля", _cars, (e) => SelectedCar = e);
                    break;
            }

        }

        protected override void _SearchCommand()
        {
            // Базовый поиск по типу оплаты, контрагенту

            _currentQuery = _defaultQuery;

            if (_selectedExpenseGroup != null && _selectedExpenseType == null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.Expense.ExpenseType.GroupID == _selectedExpenseGroup.ID);
            }

            else if (_selectedExpenseGroup != null && _selectedExpenseType != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.Expense.TypeID == _selectedExpenseType.ID);
            }

            if (_selectedCar != null)
            {
                _currentQuery = _currentQuery
                    .Where(p => p.CarID == _selectedCar.ID);
            }

            try
            {
                Entities = new ObservableCollection<CarExpense>(
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
                Entities = new ObservableCollection<CarExpense>(
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

                _baseFilter = c => true;

                _defaultQuery = JoinBaseQuery(_ctx.CarExpense);

                Entities = new ObservableCollection<CarExpense>(_defaultQuery.Take(_dataCountPerPage).ToList());

                ExpenseGroups = _ctx.ExpenseGroup
                    .Include(c => c.ExpenseType)
                    .AsNoTracking()
                    .ToList();

                Cars = new ObservableCollection<DataLayer.Car>(_ctx.Car
                    .Include(c => c.CarEquipment)
                    .Include(c => c.CarEquipment.CarModel)
                    .Include(c => c.CarEquipment.CarModel.CarBrand)
                    .AsNoTracking()
                    .ToList());

                _filter.ExpenseGroups = _expenseGroups;
                _filter.Cars = _cars;
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        #endregion Command implementation

        #region Other

        protected override IQueryable<CarExpense> JoinBaseQuery(IQueryable<CarExpense> query)
        {
            return query
                .Include(e => e.Car)
                .Include(e => e.Expense)
                .Include(e => e.Expense.ExpenseType)
                .Include(e => e.Expense.ExpenseType.ExpenseGroup)
                .AsNoTracking();
        }

        public sealed class FilterMaker : EntityFilterBase<CarExpense>
        {
            #region Filter private fields

            private IEnumerable<ExpenseGroup> _expenseGroups;
            private ObservableCollection<DataLayer.Car> _cars;
            private DataLayer.Car _selectedCar;
            private ExpenseGroup _selectedExpenseGroup;

            #endregion Filter private fields

            #region Filter properties

            public IEnumerable<ExpenseGroup> ExpenseGroups
            {
                get => _expenseGroups;
                set => SetProperty(ref _expenseGroups, value);
            }

            public ObservableCollection<DataLayer.Car> Cars
            {
                get => _cars;
                set => SetProperty(ref _cars, value);
            }

            public ExpenseType SelectedExpenseType { get; set; }
            public DataLayer.Car SelectedCar
            { 
                get => _selectedCar;
                set => SetProperty(ref _selectedCar, value); 
            }
            public ExpenseGroup SelectedExpenseGroup
            {
                get => _selectedExpenseGroup;
                set => SetProperty(ref _selectedExpenseGroup, value);
            }

            public decimal? Sum { get; set; }

            public ComparisonOperator SelectedComparisonOperator { get; set; }

            public bool ShowIsDeleted { get; set; } = false;
            public HasPaymentMode SelectedHasPaymentMode { get; set; } = HasPaymentMode.Any;

            #endregion Filter properties

            public override Expression<Func<CarExpense, bool>> MakeFilter()
            {
                var expression = defaultExpression;

                if (SelectedExpenseGroup != null && SelectedExpenseType == null)
                {
                    expression = expression
                        .And(c => c.Expense.ExpenseType.GroupID == SelectedExpenseGroup.ID);
                }
                else if (SelectedExpenseGroup != null && SelectedExpenseType != null)
                {
                    expression = expression
                       .And(c => c.Expense.TypeID == SelectedExpenseType.ID);
                }

                if (SelectedCar != null)
                {
                    expression = expression
                        .And(c => c.CarID == SelectedCar.ID);
                }

                if (Sum != null && SelectedComparisonOperator != ComparisonOperator.NOT_SET)
                {
                    switch (SelectedComparisonOperator)
                    {
                        case ComparisonOperator.Equals:
                            expression = expression.And(c => c.Expense.Sum == Sum);
                            break;
                        case ComparisonOperator.NotEquals:
                            expression = expression.And(c => c.Expense.Sum != Sum);
                            break;
                        case ComparisonOperator.GreaterThen:
                            expression = expression.And(c => c.Expense.Sum > Sum);
                            break;
                        case ComparisonOperator.GreaterThenOrEquals:
                            expression = expression.And(c => c.Expense.Sum >= Sum);
                            break;
                        case ComparisonOperator.LowerThen:
                            expression = expression.And(c => c.Expense.Sum < Sum);
                            break;
                        case ComparisonOperator.LowerThenOrEquals:
                            expression = expression.And(c => c.Expense.Sum <= Sum);
                            break;
                    }
                }

                if (ShowIsDeleted == true)
                {
                    expression = expression.And(c => c.Expense.IsDeleted == true || c.Expense.IsDeleted == false);
                }

                switch(SelectedHasPaymentMode)
                {
                    case HasPaymentMode.Has:
                        expression = expression.And(c => c.HasPayment == true);
                        break;
                    case HasPaymentMode.NoHas:
                        expression = expression.And(c => c.HasPayment == false);
                        break;
                    case HasPaymentMode.Any:
                        expression = expression.And(c => c.HasPayment == true || c.HasPayment == false);
                        break;
                }

                return expression;
            }

            private ITabService _tabService;

            public FilterMaker(ITabService tabService)
            {
                ChangeHasPaymentModeCommand = new RelayCommand<int>(_ChangeHasPaymentMode);
                OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

                _tabService = tabService;
            }

            public RelayCommand<int> ChangeHasPaymentModeCommand { get; }
            public AsyncRelayCommand<Type> OpenTypeListCommand { get; }

            private void _ChangeHasPaymentMode(int x)
            {
                SelectedHasPaymentMode = (HasPaymentMode)x;
            }

            private async Task _OpenTypeListCommand(Type type)
            {
                switch (type.Name)
                {
                    case nameof(DataLayer.Car):
                        _tabService.OpenTabForSelect<Car.Salon.CarInSalonListViewModel, DataLayer.Car>(
                            "Выбор автомобиля", _cars, (e) => SelectedCar = e);
                        break;
                }


                // executer
                // input: Type, select action
            }

            public enum HasPaymentMode
            {
                Has = 0,
                NoHas,
                Any
            }
        }

        #endregion Other
    }
}