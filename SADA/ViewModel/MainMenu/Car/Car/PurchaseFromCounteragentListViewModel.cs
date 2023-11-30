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

namespace SADA.ViewModel.MainMenu.Car.Car
{
    public class PurchaseFromCounteragentListViewModel : TabObservableObjectList<CarPaymentFromCounteragent>
    {
        #region Fields

        #region Services fields

        private readonly IDialogService _dialogService;
        private readonly IWindowService _windowService;
        private readonly ITabService _tabService;

        #endregion Services fields

        #region IEnumerables fields

        private IEnumerable<Counteragent> _counteragents;
        private IEnumerable<PaymentType> _paymentTypes;

        #endregion IEnumerables fields

        #region Selected fields

        private PaymentType _selectedPaymentType;
        private Counteragent _selectedCounteragent;

        #endregion Selected fields

        #region Other fields

        private SADAEntities _ctx;

        #endregion Other fields

        #region Filter fields

        private PayToCounteragentFilter _filter;

        #endregion Filter fields

        #endregion Fields

        #region Constructor

        public PurchaseFromCounteragentListViewModel(IDialogService dialogService, IWindowService windowService, ITabService tabService)
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

            _filter = new PayToCounteragentFilter();
        }

        protected PurchaseFromCounteragentListViewModel() { }


        #endregion Constructor

        #region Properties

        public IEnumerable<Counteragent> Counteragents
        {
            get => _counteragents;
            set => SetProperty(ref _counteragents, value);
        }

        public IEnumerable<PaymentType> PaymentTypes
        {
            get => _paymentTypes;
            set => SetProperty(ref _paymentTypes, value);
        }

        public Counteragent SelectedCounteragent
        {
            get => _selectedCounteragent;
            set => SetProperty(ref _selectedCounteragent, value);
        }

        public PaymentType SelectedPaymentType
        {
            get => _selectedPaymentType;
            set => SetProperty(ref _selectedPaymentType, value);
        }

        #region Filter properties

        public PayToCounteragentFilter Filter
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

            if (_selectedPaymentType != null)
            {
                _currentQuery = _currentQuery
                    .Where(p => p.PaymentTypeID == _selectedPaymentType.ID);
            }

            if (_selectedCounteragent != null)
            {
                _currentQuery = _currentQuery
                    .Where(p => p.CounteragentID == _selectedCounteragent.ID);
            }
            try
            {
                Entities = new ObservableCollection<CarPaymentFromCounteragent>(
                _currentQuery
                .Where(_baseFilter)
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
                Entities = new ObservableCollection<CarPaymentFromCounteragent>(
                    _currentQuery.ToList());
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
                var vm = App.Current.GetService<PurchaseFromCounteragentViewModel>();
                vm.Name = $"Изменение оплаты от контрагента за авто №{_selectedEntity.ID}";
                vm.Entity = SelectedEntity;
                vm.CurrentFormMode = FormMode.Edit;
                _tabService.OpenTab(vm);
            }
            else
            {
                var vm = App.Current.GetService<PurchaseFromCounteragentViewModel>();
                vm.Name = "Добавление оплаты от контрагента за авто";
                vm.CurrentFormMode = FormMode.Add;
                _tabService.OpenTab(vm);
            }
        }

        protected override async Task _PageUpdateCommand(HandyControl.Data.FunctionEventArgs<int> e)
        {
            try
            {
                MaxPage = _currentQuery.Count() / _dataCountPerPage;
                Entities = new ObservableCollection<CarPaymentFromCounteragent>(
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

                _baseFilter = c => c.IsDeleted == false;

                _defaultQuery = JoinBaseQuery(_ctx.CarPaymentFromCounteragent);

                Entities = new ObservableCollection<CarPaymentFromCounteragent>(_defaultQuery.Take(_dataCountPerPage).ToList());

                _filter.CounteragentGroups = _ctx.CounteragentGroup
                    .AsNoTracking()
                    .Include(c => c.Counteragent)
                    .ToList();

                _filter.Counteragents = _ctx.Counteragent
                    .ToList();

                Counteragents = _ctx.Counteragent
                    .AsNoTracking()
                    .Include(c => c.CounteragentType)
                    .Include(c => c.CounteragentGroup)
                    .Include(c => c.IndividualPerson)
                    .Include(c => c.SoloTrader)
                    .Include(c => c.JuridicalPerson)
                    .OrderByDescending(c => c.CounteragentGroup.Name)
                    .OrderByDescending(c => c.CounteragentType.Name)
                    .ToList();

                PaymentTypes = _ctx.PaymentType
                    .AsNoTracking()
                .OrderByDescending(c => c.Name)
                .ToList();

                //CounteragentGroups = ...

                MaxPage = Entities.Count() / _dataCountPerPage + 1;
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        #endregion Command implementation

        #region Other

        public IQueryable<CarPaymentFromCounteragent> JoinBaseQuery(IQueryable<CarPaymentFromCounteragent> query)
        {
            return query
                .Include(c => c.Car)
                .Include(c => c.Car.CarEquipment)
                .Include(c => c.Car.CarEquipment.CarModel)
                .Include(c => c.Car.CarEquipment.CarModel.CarBrand)
                .Include(c => c.Counteragent)
                .Include(c => c.Counteragent.CounteragentGroup)
                .Include(c => c.Counteragent.CounteragentType)
                .Include(c => c.PaymentType)
                .AsNoTracking();
        }

        public sealed class PayToCounteragentFilter : EntityFilterBase<CarPaymentFromCounteragent>
        {
            #region Filter private fields

            private CounteragentGroup _counteragentGroup;
            private IEnumerable<Counteragent> _counteragents;
            private IEnumerable<CounteragentGroup> _counteragentGroups;

            #endregion Filter private fields

            #region Filter properties

            public IEnumerable<Counteragent> CounteragenstAll;

            public IEnumerable<CounteragentGroup> CounteragentGroups
            {
                get => _counteragentGroups;
                set => SetProperty(ref _counteragentGroups, value);
            }

            public CounteragentGroup CounteragentGroup
            {
                get => _counteragentGroup;
                set
                {
                    if (SetProperty(ref _counteragentGroup, value))
                    {
                        if (_counteragentGroup == null)
                        {
                            Counteragents = CounteragenstAll;
                        }
                        else
                        {
                            Counteragents = _counteragentGroup.Counteragent;
                        }
                    }
                }
            }

            public IEnumerable<Counteragent> Counteragents
            {
                get => _counteragents;
                set => SetProperty(ref _counteragents, value);
            }

            public Counteragent Counteragent { get; set; }
            public PaymentType PaymentType { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public bool ShowIsDeleted { get; set; }

            #endregion Filter properties

            public override Expression<Func<CarPaymentFromCounteragent, bool>> MakeFilter()
            {
                var expression = defaultExpression;
                if (CounteragentGroup != null)
                {
                    expression = expression.And(c => c.Counteragent.GroupID == CounteragentGroup.ID);
                }
                if (Counteragent != null)
                {
                    expression = expression.And(c => c.CounteragentID == Counteragent.ID);
                }
                if (PaymentType != null)
                {
                    expression = expression.And(c => c.PaymentTypeID == PaymentType.ID);
                }
                if (StartDate != null)
                {
                    expression = expression.And(c => c.Date >= StartDate);
                }
                if (EndDate != null)
                {
                    expression = expression.And(c => c.Date <= EndDate);
                }
                if (ShowIsDeleted)
                {
                    expression = expression.And(c => c.IsDeleted == true || c.IsDeleted == false);
                }

                return expression;
            }
        }

        #endregion Other
    }
}