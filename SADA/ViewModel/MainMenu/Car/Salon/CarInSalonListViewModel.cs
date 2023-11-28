using CommunityToolkit.Mvvm.Input;
using DataLayer;
using SADA.Infastructure.Core;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    internal class CarInSalonListViewModel : TabObservableObjectList<DataLayer.Car>
    {
        #region Fields

        #region Services fields

        private readonly IDialogService _dialogService;
        private readonly IWindowService _windowService;
        private readonly ITabService _tabService;

        #endregion Services fields

        #region IEnumerables fields

        private IEnumerable<CarBrand> _carBrands;
        

        #endregion IEnumerables fields

        #region Selected fields

        private CarBrand _selectedCarBrand;
        private CarModel _selectedCarModel;

        #endregion

        #region Other fields

        // Добавляется ко всем базовым запросам
        private Expression<Func<DataLayer.Car, bool>> _baseFilter;

        // Базовый запрос по умолчанию, когда нет данных
        private IQueryable<DataLayer.Car> _currentQuery;

        private IQueryable<DataLayer.Car> _defaultQuery;

        // For filter
        private IQueryable<DataLayer.Car> _filterQuery;

        private SADAEntities _ctx;


        #endregion Other fields

        #region Filter fields

        private CarFilterMaker _filter;
        private IEnumerable<CarColor> _carColors;

        #endregion Filter fields



        #endregion Fields

        #region Constructor

        public CarInSalonListViewModel(IDialogService dialogService, IWindowService windowService, ITabService tabService)
        {
            CloseCommand = new RelayCommand(_OnClose);
            OpenCarFormCommand = new RelayCommand<FormMode>(_OpenCarFormCommand);

            SearchCommand = new RelayCommand(_SearchCommand);
            SaveAsFileCommand = new RelayCommand(_SaveAsFileCommand);

            ApplyFilterCommand = new RelayCommand(_ApplyFilterCommand);
            ClearFilterCommand = new RelayCommand(_ClearFilterCommand);

            _dialogService = dialogService;
            _windowService = windowService;
            _tabService = tabService;

            _filter = new CarFilterMaker();
        }

        protected CarInSalonListViewModel()
        { }

        ~CarInSalonListViewModel()
        {
            _ctx?.Dispose();
        }

        #endregion Constructor

        #region Properties

        public IEnumerable<CarBrand> CarBrands
        {
            get => _carBrands;
            set => SetProperty(ref _carBrands, value);
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

        #region Filter properties

        public CarFilterMaker Filter
        {
            get => _filter;
        }

        public IEnumerable<CarColor> CarColors
        {
            get => _carColors;
            set => SetProperty(ref _carColors, value);
        }

        #endregion

        #endregion Properties

        #region Commands

        public RelayCommand<FormMode> OpenCarFormCommand { get; }
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
            // 1  - Ничего не выбрано
            // 2 - Только бренд
            // 3 - Бренд и модель

            // 1
            if (_selectedCarBrand == null)
            {
                _currentQuery = _defaultQuery;
            }
            // 2
            else if (_selectedCarBrand != null && _selectedCarModel == null)
            {
                _currentQuery = _defaultQuery
                    .Where(c => c.CarEquipment.CarModel.CarBrand.ID == _selectedCarBrand.ID);
            }
            //3
            else
            {
                _currentQuery = _defaultQuery
                    .Where(c => c.CarEquipment.CarModel.CarBrand.ID == _selectedCarBrand.ID)
                    .Where(c => c.CarEquipment.CarModel.ID == _selectedCarModel.ID);
            }

            Entities = new ObservableCollection<DataLayer.Car>(_currentQuery.ToList());
        }

        private void _SaveAsFileCommand()
        {
        }

        private void _ApplyFilterCommand()
        {
            
            //_filterQuery = _filter.MakeQuery();
            Entities = new ObservableCollection<DataLayer.Car>(_defaultQuery.Where(_filter.MakeFilter()).ToList());
        }

        private void _ClearFilterCommand()
        {
            _filterQuery = null;
            _filter.FilterFieldsClear();
            //Entities = new ObservableCollection<DataLayer.Car>(_filterQuery.ToList());
        }

        private void _OpenCarFormCommand(FormMode parameter)
        {
            if (parameter == FormMode.Edit)
            {
                if (_selectedEntity == null)
                {
                    _dialogService.ShowMessageBox("Ошибка", "Вы не выбрали запись для редактирования", MessageBoxButton.OK);
                    return;
                }
                var vm = App.Current.GetService<CarInSalonViewModel>();
                vm.Name = "Изменение автомобиля";
                vm.Entity = SelectedEntity;
                vm.CurrentFormMode = FormMode.Edit;
                _tabService.OpenTab(vm);
            }
            else
            {
                var vm = App.Current.GetService<CarInSalonViewModel>();
                vm.Name = "Добавление автомобиля";
                vm.CurrentFormMode = FormMode.Add;
                _tabService.OpenTab(vm);
            }
        }

        protected override async Task _PageUpdateCommand(HandyControl.Data.FunctionEventArgs<int> e)
        {
            MaxPage = _currentQuery.Count() / _dataCountPerPage;
            Entities = new ObservableCollection<DataLayer.Car>(await _currentQuery.Skip((e.Info - 1) * _dataCountPerPage).Take(_dataCountPerPage).IncludeAll().ToListAsync());
        }

        protected override void LoadedInner()
        {
            _ctx = new SADAEntities();

            _baseFilter = c => c.IsDeleted == false;

            _defaultQuery = _ctx.Car
                .AsNoTracking()
                .Include(c => c.CarEquipment)
                .Include(c => c.CarEquipment.CarModel)
                .Include(c => c.CarEquipment.CarModel.CarBrand)
                .Where(_baseFilter);

            Entities = new ObservableCollection<DataLayer.Car>(_ctx.Car.IncludeAll().ToList());
            CarBrands = _ctx.CarBrand.ToList();
            CarColors = _ctx.CarColor.ToList();
            MaxPage = Entities.Count() / _dataCountPerPage + 1;
        }

        #endregion Command implementation

        #region Other

        internal class CarFilterMaker : EntityFilterBase<DataLayer.Car>
        {
            #region Filter fields

            public string VIN { get; set; }
            public CarColor CarColor { get; set; }

            #endregion Filter fields

            public override Expression<Func<DataLayer.Car, bool>> MakeFilter()
            {
                var expression = defaultExpression;
                if (VIN != null)
                {
                    expression = expression.And(c => c.VIN == VIN);
                }
                if (CarColor != null)
                {
                    expression = expression.And(c => c.CarColor.ID == CarColor.ID);
                }

                return expression;
            }

            public IQueryable<DataLayer.Car> MakeQuery()
            {
                var query = Enumerable.Empty<DataLayer.Car>().AsQueryable<DataLayer.Car>();
                if (VIN != null)
                {
                    query = query.Where(c => c.VIN == VIN);
                }
                if (CarColor != null)
                {
                    query = query.Where(c => c.CarColor.ID == CarColor.ID);
                }

                return query;
            }
        }

        #endregion Other
    }
}