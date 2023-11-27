using CommunityToolkit.Mvvm.Input;
using DataLayer;
using SADA.Infastructure.Core;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    internal class CarInSalonViewModel : TabObservableObjectForm<DataLayer.Car>
    {
        #region Fields

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        #region Main Form fields 

        private IEnumerable<Counteragent> _counteragents;
        private IEnumerable<CarEquipment> _carEquipments;
        private IEnumerable<CarColor> _carColors;
        private IEnumerable<Staff> _staffs;
        private IEnumerable<CarStatus> _carStatuses;
        private IEnumerable<PaymentType> _paymentType;
        private IEnumerable<CarDealership> _carDealerships;

        #endregion Main Form fields 

        #region Image field

        private ObservableCollection<CarPhoto> _carPhotos;

        #endregion

        #endregion Fields

        #region Constructor

        public CarInSalonViewModel(IDialogService dialogService, ITabService tabService)
        {
            CloseCommand = new RelayCommand(_OnClose);
            AddImageCommand = new RelayCommand(_AddImageCommand);
            DeleteImageCommand = new RelayCommand(_DeleteImageCommand);
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

            _dialogService = dialogService;
            _tabService = tabService;
        }

        protected CarInSalonViewModel()
        { }

        #endregion Constructor

        #region Properties

        #region Main Form properties 
        public IEnumerable<Counteragent> Counteragents
        {
            get => _counteragents;
            set => SetProperty(ref _counteragents, value);
        }

        public IEnumerable<CarEquipment> CarEquipments
        {
            get => _carEquipments;
            set => SetProperty(ref _carEquipments, value);
        }

        public IEnumerable<CarColor> CarColors
        {
            get => _carColors;
            set => SetProperty(ref _carColors, value);
        }

        public IEnumerable<Staff> Staffs
        {
            get => _staffs;
            set => SetProperty(ref _staffs, value);
        }

        public IEnumerable<CarStatus> CarStatuses
        {
            get => _carStatuses;
            set => SetProperty(ref _carStatuses, value);
        }

        public IEnumerable<PaymentType> PaymentTypes
        {
            get => _paymentType;
            set => SetProperty(ref _paymentType, value);
        }

        public IEnumerable<CarDealership> CarDealerships
        {
            get => _carDealerships;
            set => SetProperty(ref _carDealerships, value);
        }

        #endregion Main Form properties 

        #region Form image properties

        public ObservableCollection<CarPhoto> CarPhotos
        {
            get => _carPhotos;
            set => SetProperty(ref _carPhotos, value);
        }

        #endregion

        #endregion Properties

        #region Commands

        public AsyncRelayCommand<Type> OpenTypeListCommand { get; }
        public RelayCommand FormCommand { get; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand DeleteImageCommand { get; }

        #endregion Commands

        #region Command implementation

        private void _AddImageCommand()
        {
            var images = _dialogService.ShowFileDialog();
        }

        private void _DeleteImageCommand()
        {
            
        }

        private void _OnClose()
        {
            var result = _dialogService.ShowMessageBox("Вопрос", $"Закрыть вкладку {Name}?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _RaiseCloseEvent();
            }
        }

        private async Task _OpenTypeListCommand(Type type)
        {
            switch (type.Name)
            {
                case nameof(DataLayer.Car):
                    break;
            }
        }

        private void _FormCommand()
        {
            if (CurrentFormMode == FormMode.Edit || CurrentFormMode == FormMode.Add)
            {
                using (var ctx = new SADAEntities())
                {
                    ctx.Car.AddOrUpdate(Entity);
                    ctx.SaveChanges();
                }
            }
        }

        #endregion Command implementation

        #region Other

        protected override void LoadedInner()
        {
            using (var ctx = new SADAEntities())
            {
                if (CurrentFormMode == FormMode.Edit || CurrentFormMode == FormMode.See)
                {
                    if (Entity == null)
                    {
                        throw new NullReferenceException(nameof(Entity));
                    }
                    Entity = ctx.Car
                        .Include(c => c.CarColor)
                        .Include(c => c.CarStatus)
                        .Include(c => c.Counteragent)
                        .Include(c => c.Counteragent1)
                        .Include(c => c.Counteragent2)
                        .Include(c => c.CarEquipment)
                        .Include(c => c.CarDealership)
                        .Include(c => c.PaymentType)
                        .Include(c => c.Staff)
                        .Include(c => c.Staff1)
                        .Single(c => c.ID == Entity.ID);
                }
                Counteragents = ctx.Counteragent
                    .AsNoTracking()
                    .Include(c => c.CounteragentType)
                    .Include(c => c.CounteragentGroup)
                    .Include(c => c.IndividualPerson)
                    .Include(c => c.JuridicalPerson)
                    .Include(c => c.SoloTrader)
                    .OrderByDescending(c => c.CounteragentGroup.Name)
                    .OrderByDescending(c => c.CounteragentType.Name)
                    .ToList();

                CarColors = ctx.CarColor
                    .AsNoTracking()
                    .OrderByDescending(c => c.Paint)
                    .ToList();

                Staffs = ctx.Staff
                    .AsNoTracking()
                    .Include(s => s.Passport)
                    .Include(s => s.StaffPost)
                    .OrderByDescending(c => c.Passport.Surname)
                    .OrderByDescending(c => c.Passport.Name)
                    .OrderByDescending(c => c.Passport.Patronymic)
                    .ToList();

                CarEquipments = ctx.CarEquipment
                    .AsNoTracking()
                    .ToList();

                CarStatuses = ctx.CarStatus
                    .AsNoTracking()
                    .OrderByDescending(c => c.Name)
                    .ToList();

                PaymentTypes = ctx.PaymentType
                    .AsNoTracking()
                    .OrderByDescending(c => c.Name)
                    .ToList();

                CarDealerships = ctx.CarDealership
                   .AsNoTracking()
                   .OrderByDescending(c => c.Name)
                   .ToList();
            }
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
                            Entity = new DataLayer.Car();
                            break;
                    }
                }
            }
        }

        #endregion Other
    }
}