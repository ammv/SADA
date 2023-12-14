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

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    public class CarInSalonViewModel : TabObservableObjectForm<DataLayer.Car>
    {
        #region Fields

        private SADAEntities _ctx;

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        #region Main Form fields

        private ObservableCollection<Counteragent> _counteragents;
        private IEnumerable<CarEquipment> _carEquipments;
        private IEnumerable<CarColor> _carColors;
        private IEnumerable<Staff> _staffs;
        private IEnumerable<CarStatus> _carStatuses;
        private IEnumerable<PaymentType> _paymentTypes;
        private IEnumerable<CarDealership> _carDealerships;

        private IEnumerable<CarBrand> _carBrands;

        // Store models for current brand
        private IEnumerable<CarModel> _carModels;

        // For update carModels combobox
        private CarBrand _selectedCarBrand;

        // For update carEquipments combobox
        private CarModel _selectedCarModel;

        private CarEquipment _selectedCarEquipment;

        #endregion Main Form fields

        #region Image field

        private ObservableCollection<CarPhoto> _carPhotos;
        private CarPhoto _selectedCarPhoto;

        #endregion Image field

        #endregion Fields

        #region Constructor

        public CarInSalonViewModel(IDialogService dialogService, ITabService tabService)
        {

            FormCommand = new RelayCommand(_FormCommand);
            AddImageCommand = new RelayCommand(_AddImageCommand);
            DeleteImageCommand = new RelayCommand(_DeleteImageCommand);
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

            _dialogService = dialogService;
            _tabService = tabService;

            FormModeTabNameMap = new Dictionary<FormMode, Func<DataLayer.Car, string>>
            {
                {FormMode.Add, (s) => "Добавление автомобиля"},
                {FormMode.Edit, (s) => $"Изменение автомобиля {s.CarEquipment.CarModel.CarBrand.Name} {s.CarEquipment.CarModel.Name} {s.CarEquipment.Name} №{s.ID}"},
                {FormMode.See, (s) => $"Просмотр автомобиля {s.CarEquipment.CarModel.CarBrand.Name} {s.CarEquipment.CarModel.Name} {s.CarEquipment.Name} №{s.ID}"},
            };

            FormModeActionMap = new Dictionary<FormMode, Action>
            {
                {FormMode.Add,  () => _entity = new DataLayer.Car() }
            };
        }

        protected CarInSalonViewModel()
        { }

        #endregion Constructor

        #region Properties

        #region Main Form properties

        public ObservableCollection<Counteragent> Counteragents
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
            get => _paymentTypes;
            set => SetProperty(ref _paymentTypes, value);
        }

        public IEnumerable<CarDealership> CarDealerships
        {
            get => _carDealerships;
            set => SetProperty(ref _carDealerships, value);
        }

        public IEnumerable<CarBrand> CarBrands
        {
            get => _carBrands;
            set => SetProperty(ref _carBrands, value);
        }

        public IEnumerable<CarModel> CarModels
        {
            get => _carModels;
            set => SetProperty(ref _carModels, value);
        }

        public CarEquipment SelectedCarEquipment
        {
            get => _selectedCarEquipment;
            set => SetProperty(ref _selectedCarEquipment, value);
        }

        public CarBrand SelectedCarBrand
        {
            get => _selectedCarBrand;
            set => SetProperty(ref _selectedCarBrand, value);
        }

        public CarModel SelectedCarModel
        {
            get => _selectedCarModel;
            set => SetProperty(ref _selectedCarModel, value);
        }

        #endregion Main Form properties

        #region Form image properties

        public ObservableCollection<CarPhoto> CarPhotos
        {
            get => _carPhotos;
            set => SetProperty(ref _carPhotos, value);
        }

        public CarPhoto SelectedCarPhoto
        {
            get => _selectedCarPhoto;
            set => SetProperty(ref _selectedCarPhoto, value);
        }

        #endregion Form image properties

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
            var imageFiles = _dialogService.ShowFileDialog(_dialogService.FileDialogImageFilter);

            if (imageFiles == null) return;

            foreach (var imageFile in imageFiles)
            {
                File file = new File
                {
                    ContentType = imageFile.Extension,
                    Data = System.IO.File.ReadAllBytes(imageFile.FullName),
                    Name = imageFile.Name.Remove(imageFile.Name.Length - imageFile.Extension.Length),
                    FileID = Guid.NewGuid()
                };

                CarPhoto carPhoto = new CarPhoto
                {
                    Car = Entity,
                    File = file
                };

                _carPhotos.Add(carPhoto);
            }
        }

        private void _DeleteImageCommand()
        {
            if (_selectedCarPhoto == null)
            {
                _dialogService.ShowMessageBox("Ошибка", "Не выбрано изображение для удаления", MessageBoxButton.OK);
                return;
            }
            _carPhotos.Remove(_selectedCarPhoto);
        }

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
                    string msg = $"Запись об автомобиле {Entity.ID} изменена";
                    if (_currentFormMode == FormMode.Add)
                    {
                        _ctx.Car.Add(Entity);
                        msg = "Новая запись об автомобиле добавлена";
                    }

                    Entity.CarPhoto = _carPhotos;

                    _ctx.SaveChanges();

                    if (_currentFormMode == FormMode.Add)
                    {
                        Entity = new DataLayer.Car();
                    }

                    //UpdateName();

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
                Entity = _ctx.Car
                    .Include(c => c.CarEquipment)
                    .Include(c => c.CarEquipment.CarModel)
                    .Include(c => c.CarEquipment.CarModel.CarBrand)
                    .Include(c => c.CarPhoto)
                    .Include(c => c.CarPhoto.Select(cp => cp.File))
                    .FirstOrDefault(c => c.ID == Entity.ID);

                CarPhotos = new ObservableCollection<CarPhoto>(Entity.CarPhoto);
            }
            else
            {
                CarPhotos = new ObservableCollection<CarPhoto>();
            }

            CarBrands = _ctx.CarBrand
                .ToList();

            if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.See)
            {
                SelectedCarBrand = Entity.CarEquipment.CarModel.CarBrand;
                SelectedCarModel = Entity.CarEquipment.CarModel;
                SelectedCarEquipment = Entity.CarEquipment;
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

            CarColors = _ctx.CarColor
                .OrderByDescending(c => c.Paint)
                .ToList();

            Staffs = _ctx.Staff
                .OrderByDescending(c => c.Passport.Surname)
                .OrderByDescending(c => c.Passport.Name)
                .OrderByDescending(c => c.Passport.Patronymic)
                .ToList();

            CarStatuses = _ctx.CarStatus
                .OrderByDescending(c => c.Name)
                .ToList();

            PaymentTypes = _ctx.PaymentType
                .OrderByDescending(c => c.Name)
                .ToList();

            CarDealerships = _ctx.CarDealership
               .OrderByDescending(c => c.Name)
               .ToList();

            // Wait EF loading data
            Thread.Sleep(100);
        }

        #endregion Other
    }
}