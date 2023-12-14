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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.MainMenu.Car.Car
{
    public class PayToCounteragentViewModel : TabObservableObjectForm<CarPaymentToCounteragent>
    {
        #region Fields

        private SADAEntities _ctx;

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        #region Main Form fields

        private ObservableCollection<Counteragent> _counteragents;
        private IEnumerable<PaymentType> _paymentTypes;
        private ObservableCollection<DataLayer.Car> _cars;

        #endregion Main Form fields

        #endregion Fields

        #region Constructor

        public PayToCounteragentViewModel(IDialogService dialogService, ITabService tabService)
        {
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);
            FormCommand = new RelayCommand(_FormCommand);

            _dialogService = dialogService;
            _tabService = tabService;

            FormModeTabNameMap = new Dictionary<FormMode, Func<CarPaymentToCounteragent, string>>
            {
                {FormMode.Add, (s) => "Добавление оплаты контрагенту за автомобиль"},
                {FormMode.Edit, (s) => $"Изменение оплаты контрагенту за автомобиль №{s.ID}"},
                {FormMode.See, (s) => $"Просмотр оплаты контрагенту за автомобиль №{s.ID}"}
            };

            FormModeActionMap = new Dictionary<FormMode, Action>
            {
                {FormMode.Add,  () => _entity = new CarPaymentToCounteragent{ Date = DateTime.Now} }
            };
        }

        protected PayToCounteragentViewModel()
        { }

        #endregion Constructor

        #region Properties

        #region Main Form properties

        public ObservableCollection<Counteragent> Counteragents
        {
            get => _counteragents;
            set => SetProperty(ref _counteragents, value);
        }

        public IEnumerable<PaymentType> PaymentTypes
        {
            get => _paymentTypes;
            set => SetProperty(ref _paymentTypes, value);
        }

        public ObservableCollection<DataLayer.Car> Cars
        {
            get => _cars;
            set => SetProperty(ref _cars, value);
        }


        #endregion Main Form properties

        #endregion Properties

        #region Commands

        public AsyncRelayCommand<Type> OpenTypeListCommand { get; }
        public RelayCommand FormCommand { get; }

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
                    _tabService.OpenTabForSelect<Salon.CarInSalonListViewModel, DataLayer.Car>(
                        "Выбор автомобиля", _cars, (e) => Entity.Car = e);
                    break;
                case nameof(DataLayer.Counteragent):
                    _tabService.OpenTabForSelect<Home.Counteragent.CounteragentListViewModel, DataLayer.Counteragent>(
                        "Выбор контрагента", _counteragents, (e) => Entity.Counteragent = e);
                    break;
            }
        }

        private void _FormCommand()
        {
            try
            {
                if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.Add)
                {
                    string msg = "Запись об оплате контрагенту за автомобиль обновлена";
                    if (_currentFormMode == FormMode.Add)
                    {
                        _ctx.CarPaymentToCounteragent.Add(Entity);
                        msg = "Новая запись об оплате контрагенту за автомобиль добавлена";
                    }

                    _ctx.SaveChanges();

                    if(_currentFormMode == FormMode.Add)
                    {
                        Entity = new CarPaymentToCounteragent();
                        Entity.Date = DateTime.Now;
                    }

                    _dialogService.ShowMessageBox("Уведомление", msg, MessageBoxButton.OK);
                }
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
            catch (Exception ex)
            {

                _dialogService.ShowMessageBox("Ошибка", ex.Message + ex.InnerException?.Message, MessageBoxButton.OK);
            }

        }

        #endregion Command implementation

        #region Other

        protected void EnsureConnectionIsOpen()
        {
            while (_ctx?.Database.Connection.State == System.Data.ConnectionState.Connecting) { }
        }
        protected override void LoadedInner()
        {
            try
            {
                _loadedInner();
            }
            catch (DbEntityValidationException ex)
            {

                DbEntityValidationExceptionHelper.ShowException(ex);
            }
            catch (Exception ex)
            {

                _dialogService.ShowMessageBox("Ошибка", ex.Message + ex.InnerException?.Message, MessageBoxButton.OK);
            }

        }

        private void _loadedInner()
        {
            _ctx = new SADAEntities();

            EnsureConnectionIsOpen();

            if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.See)
            {
                if (_entity == null)
                {
                    throw new NullReferenceException(nameof(Entity));
                }
                Entity = _ctx.CarPaymentToCounteragent
                    .Include(c => c.Car.CarEquipment)
                    .Include(c => c.Car.CarEquipment.CarModel)
                    .Include(c => c.Car.CarEquipment.CarModel.CarBrand)
                    .FirstOrDefault(c => c.ID == Entity.ID);
            }
            else if(CurrentFormMode == FormMode.Add)
            {
                _ctx.CarPaymentToCounteragent.Add(Entity);
            }

            if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.See)
            {
                //SelectedCarBrand = Entity.CarEquipment.CarModel.CarBrand;
                //SelectedCarModel = Entity.CarEquipment.CarModel;
                //SelectedCarEquipment = Entity.CarEquipment;
            }

            Counteragents = new ObservableCollection<Counteragent>(_ctx.Counteragent
                .Include(c => c.CounteragentType)
                .Include(c => c.CounteragentGroup)
                .Include(c => c.IndividualPerson)
                .Include(c => c.SoloTrader)
                .Include(c => c.JuridicalPerson)
                .OrderByDescending(c => c.CounteragentGroup.Name)
                .OrderByDescending(c => c.CounteragentType.Name)
                .ToList());

            PaymentTypes = _ctx.PaymentType
                .OrderByDescending(c => c.Name)
                .ToList();

            Cars = new ObservableCollection<DataLayer.Car>( _ctx.Car
                    .Include(c => c.CarEquipment)
                    .Include(c => c.CarEquipment.CarModel)
                    .Include(c => c.CarEquipment.CarModel.CarBrand)
                    .ToList());

            // Wait EF loading data
            Thread.Sleep(100);
        }

        #endregion Other
    }
}