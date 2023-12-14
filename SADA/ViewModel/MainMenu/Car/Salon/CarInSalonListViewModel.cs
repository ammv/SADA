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

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    public class CarInSalonListViewModel : TabObservableObjectList<DataLayer.Car>
    {
        #region Fields

        #region Services fields

        #endregion Services fields

        #region IEnumerables fields

        private IEnumerable<CarBrand> _carBrands;
        

        #endregion IEnumerables fields

        #region Selected fields

        private CarBrand _selectedCarBrand;
        private CarModel _selectedCarModel;

        #endregion

        #region Other fields

        private SADAEntities _ctx;

        #endregion Other fields

        #region Filter fields

        private CarFilterMaker _filter;
        private IEnumerable<CarColor> _carColors;

        #endregion Filter fields

        #endregion Fields

        #region Constructor

        public CarInSalonListViewModel(IDialogService dialogService, ITabService tabService):
            base(dialogService, tabService, TypeWrapper<TabObservableObjectForm<DataLayer.Car>>.Make<CarInSalonViewModel>())
        {
            //AddTabName = (e) => "Добавление автомобиля";
            //EditTabName = (e) => $"Изменение автомобиля №{e.ID}";

            _filter = new CarFilterMaker();

            ListModeMap = new Dictionary<ListMode, string>
            {
                {ListMode.Default, "Автомобили в салоне" },
                {ListMode.Select, "Выбор автомобиля в салоне" },
            };
        }

        protected CarInSalonListViewModel(): base(null, null, null)
        { }

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

            try
            {
                Entities = new ObservableCollection<DataLayer.Car>(
                _currentQuery
                .Where(_baseFilter)
                .ToList());
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
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
                Entities = new ObservableCollection<DataLayer.Car>(
                    _currentQuery.ToList());
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

                _defaultQuery = JoinBaseQuery(_ctx.Car);

                Entities = new ObservableCollection<DataLayer.Car>(_defaultQuery.Take(_dataCountPerPage).ToList());

                CarBrands = _ctx.CarBrand.ToList();
                CarColors = _ctx.CarColor.ToList();

                MaxPage = Entities.Count() / _dataCountPerPage + 1;
            }
            catch(DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
            
        }

        #endregion Command implementation

        #region Other

        protected override IQueryable<DataLayer.Car> JoinBaseQuery(IQueryable<DataLayer.Car> query)
        {
            return query
                .Include(c => c.CarStatus)
                .Include(c => c.CarEquipment)
                .Include(c => c.CarEquipment.CarModel)
                .Include(c => c.CarEquipment.CarModel.CarBrand)
                .AsNoTracking();
        }

        public sealed class CarFilterMaker : EntityFilterBase<DataLayer.Car>
        {
            #region Filter private fields
            private CarBrand _carBrand;
            #endregion
            #region Filter properties

            public string VIN { get; set; }
            public CarColor CarColor { get; set; }
            public CarBrand CarBrand 
            { 
                get => _carBrand;
                set => SetProperty(ref _carBrand, value);
            }
            public CarModel CarModel { get; set; }
            public bool? IsDeleted { get; set; }

            #endregion Filter properties

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
                if (CarBrand != null)
                {
                    expression = expression.And(c => c.CarEquipment.CarModel.CarBrand.ID == CarBrand.ID);
                }
                if (CarModel != null)
                {
                    expression = expression.And(c => c.CarEquipment.CarModel.ID == CarModel.ID);
                }
                if (IsDeleted != null)
                {
                    expression = expression.And(c => c.IsDeleted == true || c.IsDeleted == false);
                }

                return expression;
            }
        }

        #endregion Other
    }
}