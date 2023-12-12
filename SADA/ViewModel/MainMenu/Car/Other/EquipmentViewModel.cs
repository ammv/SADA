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

namespace SADA.ViewModel.MainMenu.Car.Other
{
    public class EquipmentViewModel : TabObservableObjectForm<CarEquipment>
    {
        #region Fields

        private SADAEntities _ctx;

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        #region Main Form fields

        private IEnumerable<CarBody> _carBodies;
        private IEnumerable<CarTransmission> _carTransmissions;
        private IEnumerable<CarSteeringWheelPosition> _carSteeringWheelPositions;
        private IEnumerable<CarFuel> _carFuels;
        private IEnumerable<CarEngine> _carEngines;
        private IEnumerable<CarDrive> _carDrives;

        private CollectionWithSelection<CarBrand> _carBrands;

        private ObservableCollection<CarEquipmentGroupAttribute> _carEquipmentGroupAttributes;

        #endregion Main Form fields

        #endregion Fields

        #region Constructor

        public EquipmentViewModel(IDialogService dialogService, ITabService tabService)
        {
            FormCommand = new RelayCommand(_FormCommand);
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

            _dialogService = dialogService;
            _tabService = tabService;
        }

        protected EquipmentViewModel()
        { }

        #endregion Constructor

        #region Properties

        #region Main Form properties

        public ObservableCollection<CarEquipmentGroupAttribute> CarEquipmentGroupAttributes
        {
            get => _carEquipmentGroupAttributes;
            set => SetProperty(ref _carEquipmentGroupAttributes, value);
        }

        public IEnumerable<CarBody> CarBodies
        {
            get => _carBodies;
            set => SetProperty(ref _carBodies, value);
        }

        public CollectionWithSelection<CarBrand> CarBrands
        {
            get => _carBrands;
            set => SetProperty(ref _carBrands, value);
        }

        public IEnumerable<CarTransmission> CarTransmissions
        {
            get => _carTransmissions;
            set => SetProperty(ref _carTransmissions, value);
        }

        public IEnumerable<CarSteeringWheelPosition> CarSteeringWheelPositions
        {
            get => _carSteeringWheelPositions;
            set => SetProperty(ref _carSteeringWheelPositions, value);
        }

        public IEnumerable<CarFuel> CarFuels
        {
            get => _carFuels;
            set => SetProperty(ref _carFuels, value);
        }

        public IEnumerable<CarEngine> CarEngines
        {
            get => _carEngines;
            set => SetProperty(ref _carEngines, value);
        }

        public IEnumerable<CarDrive> CarDrives
        {
            get => _carDrives;
            set => SetProperty(ref _carDrives, value);
        }


        //public ExpenseGroup SelectedExpenseGroup
        //{
        //    get => _selectedExpenseGroup;
        //    set => SetProperty(ref _selectedExpenseGroup, value);
        //}


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
                //case nameof(DataLayer.Car):
                //    task = Task.Run(() => _tabService.OpenTabForSelect<Salon.CarInSalonListViewModel, DataLayer.Car>(
                //        "Выбор автомобиля", _cars, (e) => Entity.Car = e));
                //    break;
                //case nameof(DataLayer.Counteragent):
                //    task = Task.Run(() =>
                //    _tabService.OpenTabForSelect<Home.Counteragent.CounteragentListViewModel, DataLayer.Counteragent>(
                //        "Выбор контрагента", _counteragents, (e) => Entity.Counteragent = e));
                //    break;
            }
        }

        private void _FormCommand()
        {
            try
            {
                if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.Add)
                {
                    string msg = $"Запись комплектакции авто №{Entity.ID} изменена";
                    if (_currentFormMode == FormMode.Add)
                    {
                        _ctx.CarEquipment.Add(Entity);
                        msg = "Новая запись об комплектации авто добавлена";
                    }

                    _ctx.SaveChanges();

                    if (_currentFormMode == FormMode.Add)
                    {
                        Entity = new CarEquipment();
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
                Entity = _ctx.CarEquipment
                    .Include(s => s.CarModel)
                    .Include(s => s.CarTransmission)
                    .Include(s => s.CarBody)
                    .Include(s => s.CarSteeringWheelPosition)
                    .Include(s => s.CarFuel)
                    .Include(s => s.CarEngine)
                    .Include(s => s.CarDrive)
                    .FirstOrDefault(c => c.ID == Entity.ID);
            }
            else if (CurrentFormMode == FormMode.Add)
            {
                _ctx.CarEquipment.Add(Entity);
            }

            CarBrands = new CollectionWithSelection<CarBrand>(_ctx.CarBrand.Include(c => c.CarModel).ToList());

            if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.See)
            {
                //CarEquipmentGroupAttributes = Entity.CarGroupAttribute.
                // SelectedExpenseGroup = Entity.ExpenseType.ExpenseGroup;
                CarBrands.Selected = Entity.CarModel.CarBrand;
            }

            CarTransmissions = _ctx.CarTransmission.ToList();
            CarBodies = _ctx.CarBody.ToList();
            CarSteeringWheelPositions = _ctx.CarSteeringWheelPosition.ToList();
            CarFuels = _ctx.CarFuel.ToList();
            CarEngines = _ctx.CarEngine.ToList();
            CarDrives = _ctx.CarDrive.ToList();

            // Wait EF loading data
            Thread.Sleep(100);
        }

        public override FormMode CurrentFormMode
        {
            get => _currentFormMode;
            set
            {
                if (SetProperty(ref _currentFormMode, value))
                {
                    switch (value)
                    {
                        case FormMode.Add:
                            _entity = new CarEquipment();
                            break;
                    }
                }
            }
        }

        #endregion Other
    }
}