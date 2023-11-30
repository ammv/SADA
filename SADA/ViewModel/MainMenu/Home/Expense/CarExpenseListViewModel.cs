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

        private readonly IDialogService _dialogService;
        private readonly IWindowService _windowService;
        private readonly ITabService _tabService;

        #endregion Services fields

        #region IEnumerables fields

        private IEnumerable<ExpenseGroup> _expenseGroups;
        private IEnumerable<DataLayer.Car> _cars;

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

        public CarExpenseListViewModel(IDialogService dialogService, IWindowService windowService, ITabService tabService)
        {
            CloseCommand = new RelayCommand(_OnClose);
            OpenEntityFormCommand = new RelayCommand<FormMode>(_OpenEntityFormCommand);

            SearchCommand = new RelayCommand(_SearchCommand);
            SaveAsFileCommand = new RelayCommand(_SaveAsFileCommand);

            ApplyFilterCommand = new RelayCommand(_ApplyFilterCommand);
            ClearFilterCommand = new RelayCommand(_ClearFilterCommand);

            

            _dialogService = dialogService;
            _windowService = windowService;
            _tabService = tabService;

            _filter = new FilterMaker(_tabService);
        }

        protected CarExpenseListViewModel() { }


        #endregion Constructor

        #region Properties

        public IEnumerable<ExpenseGroup> ExpenseGroups
        {
            get => _expenseGroups;
            set => SetProperty(ref _expenseGroups, value);
        }

        public IEnumerable<DataLayer.Car> Cars
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

        public RelayCommand<FormMode> OpenEntityFormCommand { get; }
        public RelayCommand SearchCommand { get; }
        public RelayCommand SaveAsFileCommand { get; }

        public RelayCommand ApplyFilterCommand { get; }
        public RelayCommand ClearFilterCommand { get; }
        public RelayCommand<Type> OpenTypeListCommand { get; }

        #endregion Commands

        #region Command implementation

        private void _OnClose()
        {
            var result = _dialogService.ShowMessageBox("Вопрос", $"Закрыть вкладку {Name}?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _RaiseCloseEvent();
                _ctx?.Dispose();
            }
        }

        private void _SearchCommand()
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

        private void _SaveAsFileCommand()
        {
        }

        private void _ApplyFilterCommand()
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

        private void _ClearFilterCommand()
        {
            _filter.FilterFieldsClear();
        }

        private void _OpenEntityFormCommand(FormMode parameter)
        {
            if (parameter == FormMode.Edit)
            {
                if (_selectedEntity == null)
                {
                    _dialogService.ShowMessageBox("Ошибка", "Вы не выбрали запись для редактирования", MessageBoxButton.OK);
                    return;
                }
                var vm = App.Current.GetService<CarExpenseViewModel>();
                vm.Name = $"Изменение расхода на автомобиль №{_selectedEntity.ID}";
                vm.Entity = SelectedEntity;
                vm.CurrentFormMode = FormMode.Edit;
                _tabService.OpenTab(vm);
            }
            else
            {
                var vm = App.Current.GetService<CarExpenseViewModel>();
                vm.Name = "Добавление расхода на автомобиль";
                vm.CurrentFormMode = FormMode.Add;
                _tabService.OpenTab(vm);
            }
        }

        protected override async Task _PageUpdateCommand(HandyControl.Data.FunctionEventArgs<int> e)
        {
            try
            {
                MaxPage = _currentQuery.Count() / _dataCountPerPage;
                Entities = new ObservableCollection<CarExpense>(
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

                Cars = _ctx.Car
                    .Include(c => c.CarEquipment)
                    .Include(c => c.CarEquipment.CarModel)
                    .Include(c => c.CarEquipment.CarModel.CarBrand)
                    .AsNoTracking()
                    .ToList();

                _filter.ExpenseGroups = _expenseGroups;
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        #endregion Command implementation

        #region Other

        public IQueryable<CarExpense> JoinBaseQuery(IQueryable<CarExpense> query)
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
            private IEnumerable<DataLayer.Car> _cars;
            private ExpenseGroup _selectedExpenseGroup;

            #endregion Filter private fields

            #region Filter properties

            public IEnumerable<ExpenseGroup> ExpenseGroups
            {
                get => _expenseGroups;
                set => SetProperty(ref _expenseGroups, value);
            }

            public IEnumerable<DataLayer.Car> Cars
            {
                get => _cars;
                set => SetProperty(ref _cars, value);
            }

            public ExpenseType SelectedExpenseType { get; set; }
            public DataLayer.Car SelectedCar { get; set; }
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
                OpenTypeListCommand = new RelayCommand<Type>(_OpenTypeListCommand);

                _tabService = tabService;
            }

            public RelayCommand<int> ChangeHasPaymentModeCommand { get; }
            public RelayCommand<Type> OpenTypeListCommand { get; }

            private void _ChangeHasPaymentMode(int x)
            {
                SelectedHasPaymentMode = (HasPaymentMode)x;
            }

            private void _OpenTypeListCommand(Type type)
            {
                switch (type.Name)
                {
                    case nameof(DataLayer.Car):
                        var vm = App.Current.GetService<Car.Salon.CarInSalonListViewModel>();
                        vm.Name = "Выбор автомобиля";
                        vm.CurrentListMode = ListMode.Select;
                        vm.SelectAction = (car) =>
                        {
                            SelectedCar = _cars.FirstOrDefault(c => c.ID == car.ID);
                        };
                        _tabService.OpenTab(vm);
                        break;
                }
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