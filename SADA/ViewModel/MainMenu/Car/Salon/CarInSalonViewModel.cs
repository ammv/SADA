using CommunityToolkit.Mvvm.Input;
using DataLayer;
using SADA.Infastructure.Core;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    internal class CarInSalonViewModel : TabObservableObjectForm<DataLayer.Car>
    {
        #region Fields

        private SADAEntities _ctx;

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        #region Main Form fields

        private IEnumerable<Counteragent> _counteragents;
        private IEnumerable<CarEquipment> _carEquipments;
        private IEnumerable<CarEquipment> _carEquipmentsAll;
        private IEnumerable<CarColor> _carColors;
        private IEnumerable<Staff> _staffs;
        private IEnumerable<CarStatus> _carStatuses;
        private IEnumerable<PaymentType> _paymentType;
        private IEnumerable<CarDealership> _carDealerships;

        private IEnumerable<CarBrand> _carBrands;

        // Store models for current brand
        private IEnumerable<CarModel> _carModels;

        private IEnumerable<CarModel> _carModelsAll;

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
            CloseCommand = new RelayCommand(_OnClose);
            FormCommand = new RelayCommand(_FormCommand);
            AddImageCommand = new RelayCommand(_AddImageCommand);
            DeleteImageCommand = new RelayCommand(_DeleteImageCommand);
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

            _dialogService = dialogService;
            _tabService = tabService;
        }

        protected CarInSalonViewModel()
        { }

        ~CarInSalonViewModel()
        {
            _ctx?.Dispose();
        }

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
            set
            {
                if (SetProperty(ref _selectedCarEquipment, value))
                {
                    Entity.CarEquipment = _selectedCarEquipment;
                    Entity.EquipmentID = _selectedCarEquipment.ID;
                }
            }
        }

        public CarBrand SelectedCarBrand
        {
            get => _selectedCarBrand;
            set
            {
                if (SetProperty(ref _selectedCarBrand, value))
                {
                    //CarModels = _carModelsAll.Where(c => c.CarBrand == value).ToList();
                }
            }
        }

        public CarModel SelectedCarModel
        {
            get => _selectedCarModel;
            set
            {
                if (SetProperty(ref _selectedCarModel, value))
                {
                    //CarEquipments = _carEquipmentsAll.Where(c => c.CarModel == value).ToList();
                }
            }
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

        private void _OnClose()
        {
            var result = _dialogService.ShowMessageBox("Вопрос", $"Закрыть вкладку {Name}?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _RaiseCloseEvent();
                _ctx?.Dispose();
                _ctx = null;
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
                if (CurrentFormMode == FormMode.Edit)
                {
                    //ctx.Car.Attach(Entity);
                    //ctx.Entry(Entity).State = EntityState.Modified;
                }
                else
                {
                    _ctx.Car.Add(Entity);
                }

                Entity.CarPhoto = _carPhotos;
                //ctx.CarPhoto.AddRange(Entity.CarPhoto);

                _ctx.SaveChanges();

                if (CurrentFormMode == FormMode.Add)
                {
                    _dialogService.ShowMessageBox("Уведомление", "Новая запись об автомобиле добавлена", MessageBoxButton.OK);
                }
                else if (CurrentFormMode == FormMode.Edit)
                {
                    _dialogService.ShowMessageBox("Уведомление", "Запись об автомобиле изменена", MessageBoxButton.OK);
                }
            }
        }

        #endregion Command implementation

        #region Other

        protected override void LoadedInner()
        {
            _ctx = new SADAEntities();
            if (CurrentFormMode == FormMode.Edit || CurrentFormMode == FormMode.See)
            {
                if (Entity == null)
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
                //.Include(c => c.CarColor)
                //.Include(c => c.CarStatus)
                //.Include(c => c.Counteragent)
                //.Include(c => c.Counteragent1)
                //.Include(c => c.Counteragent2)

                //.Include(c => c.CarDealership)
                //.Include(c => c.PaymentType)
                //.Include(c => c.Staff)
                //.Include(c => c.Staff1)
                

                CarPhotos = new ObservableCollection<CarPhoto>(Entity.CarPhoto);
            }
            else
            {
                CarPhotos = new ObservableCollection<CarPhoto>();
            }

            CarBrands = _ctx.CarBrand
                //.AsNoTracking()
                //.Include(c => c.CarModel)
                //.Include(c => c.CarModel.Select(c2 => c2.CarEquipment))
                .ToList();

            //_carModelsAll = ctx.CarModel
            //    .AsNoTracking()
            //    .Include(c => c.CarBrand)
            //    .ToList();

            //_carEquipmentsAll = ctx.CarEquipment
            //    .AsNoTracking()
            //    .Include(c => c.CarModel)
            //    .ToList();

            if (CurrentFormMode == FormMode.Edit || CurrentFormMode == FormMode.See)
            {
                SelectedCarBrand = Entity.CarEquipment.CarModel.CarBrand;
                SelectedCarModel = Entity.CarEquipment.CarModel;
                SelectedCarEquipment = Entity.CarEquipment;
            }

            Counteragents = _ctx.Counteragent
                //.AsNoTracking()
                //.Include(c => c.CounteragentType)
                //.Include(c => c.CounteragentGroup)
                //.Include(c => c.IndividualPerson)
                //.Include(c => c.JuridicalPerson)
                //.Include(c => c.SoloTrader)
                .OrderByDescending(c => c.CounteragentGroup.Name)
                .OrderByDescending(c => c.CounteragentType.Name)
                .ToList();

            CarColors = _ctx.CarColor
                //.AsNoTracking()
                .OrderByDescending(c => c.Paint)
                .ToList();

            Staffs = _ctx.Staff
                //.AsNoTracking()
                //.Include(s => s.Passport)
                //.Include(s => s.StaffPost)
                .OrderByDescending(c => c.Passport.Surname)
                .OrderByDescending(c => c.Passport.Name)
                .OrderByDescending(c => c.Passport.Patronymic)
                .ToList();

            CarStatuses = _ctx.CarStatus
                //.AsNoTracking()
                .OrderByDescending(c => c.Name)
                .ToList();

            PaymentTypes = _ctx.PaymentType
                //.AsNoTracking()
                .OrderByDescending(c => c.Name)
                .ToList();

            CarDealerships = _ctx.CarDealership
               //.AsNoTracking()
               .OrderByDescending(c => c.Name)
               .ToList();

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
                            Entity = new DataLayer.Car();
                            break;
                    }
                }
            }
        }

        #endregion Other
    }
}