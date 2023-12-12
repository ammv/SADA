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
    public class GeneralExpenseListViewModel : TabObservableObjectList<DataLayer.Expense>
    {
        #region Fields

        #region Services fields

        #endregion Services fields

        #region IEnumerables fields

        private IEnumerable<ExpenseGroup> _expenseGroups;

        #endregion IEnumerables fields

        #region Selected fields

        private ExpenseType _selectedExpenseType;
        private ExpenseGroup _selectedExpenseGroup;

        #endregion Selected fields

        #region Other fields

        private SADAEntities _ctx;

        #endregion Other fields

        #region Filter fields

        private FilterMaker _filter;

        #endregion Filter fields

        #endregion Fields

        #region Constructor

        public GeneralExpenseListViewModel(IDialogService dialogService, ITabService tabService):
            base(dialogService, tabService, TypeWrapper<TabObservableObjectForm<DataLayer.Expense>>.Make<GeneralExpenseViewModel>())
        {

            EditTabName = (e) => $"Изменение общего расхода №{e.ID}";
            AddTabName = (e) => "Добавление общего расхода";

            _filter = new FilterMaker();
        }

        protected GeneralExpenseListViewModel(): base(null, null, null)
        { }


        #endregion Constructor

        #region Properties

        public IEnumerable<ExpenseGroup> ExpenseGroups
        {
            get => _expenseGroups;
            set => SetProperty(ref _expenseGroups, value);
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

            if (_selectedExpenseGroup != null && _selectedExpenseType == null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.ExpenseType.GroupID == _selectedExpenseGroup.ID);
            }

            else if(_selectedExpenseGroup != null && _selectedExpenseType != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.TypeID == _selectedExpenseType.ID);
            }

            try
            {
                Entities = new ObservableCollection<DataLayer.Expense>(
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
                Entities = new ObservableCollection<DataLayer.Expense>(
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

                _defaultQuery = JoinBaseQuery(_ctx.Expense);

                Entities = new ObservableCollection<DataLayer.Expense>(_defaultQuery.Take(_dataCountPerPage).ToList());

                ExpenseGroups = _ctx.ExpenseGroup
                    .Include(c => c.ExpenseType)
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

        protected override IQueryable<DataLayer.Expense> JoinBaseQuery(IQueryable<DataLayer.Expense> query)
        {
            return query
                .Include(e => e.ExpenseType)
                .Include(e => e.ExpenseType.ExpenseGroup)
                .AsNoTracking();
        }

        public sealed class FilterMaker : EntityFilterBase<DataLayer.Expense>
        {
            #region Filter private fields

            private IEnumerable<ExpenseGroup> _expenseGroups;
            private ExpenseGroup _selectedExpenseGroup;

            #endregion Filter private fields

            #region Filter properties

            public IEnumerable<ExpenseGroup> ExpenseGroups
            { 
                get => _expenseGroups;
                set => SetProperty(ref _expenseGroups, value);
            }

            public ExpenseType SelectedExpenseType { get; set; }
            public ExpenseGroup SelectedExpenseGroup
            { 
                get => _selectedExpenseGroup;
                set => SetProperty(ref _selectedExpenseGroup, value);
            }

            public decimal? Sum { get; set; }

            public ComparisonOperator SelectedComparisonOperator { get; set; }

            public bool ShowIsDeleted { get; set; } = false;

            #endregion Filter properties

            public override Expression<Func<DataLayer.Expense, bool>> MakeFilter()
            {
                var expression = defaultExpression;

                if (SelectedExpenseGroup != null && SelectedExpenseType == null)
                {
                    expression = expression
                        .And(c => c.ExpenseType.GroupID == SelectedExpenseGroup.ID);
                }
                else if (SelectedExpenseGroup != null && SelectedExpenseType != null)
                {
                    expression = expression
                       .And(c => c.TypeID == SelectedExpenseType.ID);
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
               
                if(ShowIsDeleted == true)
                {
                    expression = expression.And(c => c.IsDeleted == true || c.IsDeleted == false);
                }

                return expression;
            }
        }

        #endregion Other
    }
}