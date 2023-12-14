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

namespace SADA.ViewModel.MainMenu.SalaryAndStaff.Salary
{
    public class AccrualOfSalariesListViewModel : TabObservableObjectList<DataLayer.Salary>
    {
        #region Fields

        #region Services fields

        #endregion Services fields

        #region IEnumerables fields

        private CollectionWithSelection<DataLayer.Staff> _staffs;
        private CollectionWithSelection<SalaryType> _salaryTypes;
        private CollectionWithSelection<DataLayer.Car> _cars;

        #endregion IEnumerables fields

        #region Selected fields

        #endregion Selected fields

        #region Other fields

        private SADAEntities _ctx;

        #endregion Other fields

        #region Filter fields

        private FilterMaker _filter;

        #endregion Filter fields

        #endregion Fields

        #region Constructor

        public AccrualOfSalariesListViewModel(IDialogService dialogService, ITabService tabService):
            base(dialogService, tabService, TypeWrapper<TabObservableObjectForm<DataLayer.Salary>>.Make<AccrualOfSalariesViewModel>())
        {

            //EditTabName = (e) => $"Изменение начисления зарплаты №{_selectedEntity.ID}";
            //AddTabName = (e) => "Добавление записи об начислении зарплаты";

            _filter = new FilterMaker(_tabService);

            ListModeMap = new Dictionary<ListMode, string>
            {
                {ListMode.Default, "Начисления зарплат" },
                {ListMode.Select, "Выбор начисления зарплаты" },
            };
        }

        protected AccrualOfSalariesListViewModel(): base(null, null, null)
        { }

        #endregion Constructor

        #region Properties

        public CollectionWithSelection<DataLayer.Staff> Staffs
        {
            get => _staffs;
            set => SetProperty(ref _staffs, value);
        }

        public CollectionWithSelection<SalaryType> SalaryTypes
        {
            get => _salaryTypes;
            set => SetProperty(ref _salaryTypes, value);
        }
        public CollectionWithSelection<DataLayer.Car> Cars
        {
            get => _cars;
            set => SetProperty(ref _cars, value);
        }


        #region Filter properties

        public FilterMaker Filter
        {
            get => _filter;
        }

        #endregion Filter properties

        #endregion Properties

        #region Commands

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

            if (_staffs.Selected != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.StaffID == _staffs.Selected.ID);
            }

            if (_cars.Selected != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.CarID == _cars.Selected.ID);
            }

            if (_salaryTypes.Selected != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.TypeID == _salaryTypes.Selected.ID);
            }

            try
            {
                Entities = new ObservableCollection<DataLayer.Salary>(
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
                Entities = new ObservableCollection<DataLayer.Salary>(
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

                _defaultQuery = JoinBaseQuery(_ctx.Salary);

                Entities = new ObservableCollection<DataLayer.Salary>(
                    _defaultQuery
                    .Where(_baseFilter)
                    .Take(_dataCountPerPage).ToList());

                Cars = new CollectionWithSelection<DataLayer.Car>(_ctx.Car
                    .Include(c => c.CarEquipment)
                    .Include(c => c.CarEquipment.CarModel)
                    .Include(c => c.CarEquipment.CarModel.CarBrand)
                    .AsNoTracking()
                    .ToList());

                SalaryTypes = new CollectionWithSelection<SalaryType>(_ctx.SalaryType
                   .AsNoTracking()
                   .ToList());

                
                Staffs = new CollectionWithSelection<DataLayer.Staff>(_ctx.Staff
                .Include(s => s.StaffRole)
                .Include(s => s.StaffPost)
                .Include(s => s.Passport)
                   .AsNoTracking()
                   .ToList());


                _filter.Staffs = Staffs.Clone();
                _filter.SalaryTypes = SalaryTypes.Clone();
                _filter.Cars = Cars.Clone();
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        #endregion Command implementation

        #region Other

        protected override IQueryable<DataLayer.Salary> JoinBaseQuery(IQueryable<DataLayer.Salary> query)
        {
            return query
                .Include(s => s.Staff)
                .Include(s => s.SalaryType)
                .Include(s => s.Car)
                .AsNoTracking();
        }

        public sealed class FilterMaker : EntityFilterBase<DataLayer.Salary>
        {
            #region Filter private fields

            private CollectionWithSelection<DataLayer.Staff> _staffs;
            private CollectionWithSelection<SalaryType> _salaryTypes;
            private CollectionWithSelection<DataLayer.Car> _cars;
            private readonly ITabService _tabService;

            #endregion Filter private fields

            #region Filter properties

            public CollectionWithSelection<DataLayer.Staff> Staffs
            {
                get => _staffs;
                set => SetProperty(ref _staffs, value);
            }

            public CollectionWithSelection<SalaryType> SalaryTypes
            {
                get => _salaryTypes;
                set => SetProperty(ref _salaryTypes, value);
            }
            public CollectionWithSelection<DataLayer.Car> Cars
            {
                get => _cars;
                set => SetProperty(ref _cars, value);
            }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public decimal? Sum { get; set; }

            public ComparisonOperator SelectedComparisonOperator { get; set; }

            public bool ShowIsDeleted { get; set; } = false;

            #endregion Filter properties

            public override Expression<Func<DataLayer.Salary, bool>> MakeFilter()
            {
                var expression = defaultExpression;

                if(StartDate != null)
                {
                    expression = expression.And(s => s.Date >= StartDate);
                }

                if (EndDate != null)
                {
                    expression = expression.And(s => s.Date >= EndDate);
                }

                if (Cars?.Selected != null)
                {
                    expression = expression.And(s => s.CarID == Cars.Selected.ID);
                }

                if (SalaryTypes?.Selected != null)
                {
                    expression = expression.And(s => s.TypeID == SalaryTypes.Selected.ID);
                }

                if (Staffs?.Selected != null)
                {
                    expression = expression.And(s => s.StaffID == Staffs.Selected.ID);
                }

                if (Sum != null && SelectedComparisonOperator != ComparisonOperator.NOT_SET)
                {
                    switch (SelectedComparisonOperator)
                    {
                        case ComparisonOperator.Equals:
                            expression = expression.And(c => c.Sum == Sum);
                            break;
                        case ComparisonOperator.NotEquals:
                            expression = expression.And(c => c.Sum != Sum);
                            break;
                        case ComparisonOperator.GreaterThen:
                            expression = expression.And(c => c.Sum > Sum);
                            break;
                        case ComparisonOperator.GreaterThenOrEquals:
                            expression = expression.And(c => c.Sum >= Sum);
                            break;
                        case ComparisonOperator.LowerThen:
                            expression = expression.And(c => c.Sum < Sum);
                            break;
                        case ComparisonOperator.LowerThenOrEquals:
                            expression = expression.And(c => c.Sum <= Sum);
                            break;
                    }
                }

                if (ShowIsDeleted == true)
                {
                    expression = expression.And(c => c.IsDeleted == true || c.IsDeleted == false);
                }

                return expression;
            }

            public FilterMaker(ITabService tabService)
            {
                OpenTypeListCommand = new RelayCommand<Type>(_OpenTypeListCommand);
                _tabService = tabService;
            }

            private void _OpenTypeListCommand(Type type)
            {
                switch (type.Name)
                {
                    case nameof(DataLayer.Car):
                        _tabService.OpenTabForSelect<Car.Salon.CarInSalonListViewModel, DataLayer.Car>(
                            "Выбор автомобиля", _cars.Collection, (e) => _cars.Selected = e);
                        break;
                }
            }

            public RelayCommand<Type> OpenTypeListCommand { get; }
        }

        #endregion Other
    }
}