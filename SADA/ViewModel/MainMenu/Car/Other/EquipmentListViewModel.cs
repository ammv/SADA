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

namespace SADA.ViewModel.MainMenu.Car.Other
{
    public class EquipmentListViewModel : TabObservableObjectList<CarEquipment>
    {
        #region Fields

        #region Services fields

        #endregion Services fields

        #region IEnumerables fields

        private CollectionWithSelection<CarBrand> _carBrands;
        private CarModel _selectedCarModel;

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

        public EquipmentListViewModel(IDialogService dialogService, ITabService tabService)
            :base(dialogService, tabService, TypeWrapper<TabObservableObjectForm<CarEquipment>>.Make<EquipmentViewModel>())
        {

            AddTabName = (e) => "Добавление комплектации автомобиля";
            EditTabName = (e) => $"Изменение комплектации автомобиля №{e.ID}";

            _filter = new FilterMaker();
        }

        protected EquipmentListViewModel(): base(null, null, null)
        { }

        #endregion Constructor

        #region Properties

        public CollectionWithSelection<CarBrand> CarBrands
        {
            get => _carBrands;
            set => SetProperty(ref _carBrands, value);
        }

        public CarModel SelectedCarModel
        {
            get => _selectedCarModel;
            set => SetProperty(ref _selectedCarModel, value);
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

            if (_carBrands.Selected != null && _selectedCarModel == null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.CarModel.CarBrandID == _carBrands.Selected.ID);
            }

            if (_selectedCarModel != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.ModelID == _selectedCarModel.ID);
            }

            try
            {
                Entities = new ObservableCollection<CarEquipment>(
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
                Entities = new ObservableCollection<CarEquipment>(
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

                _defaultQuery = JoinBaseQuery(_ctx.CarEquipment);

                Entities = new ObservableCollection<CarEquipment>(
                    _defaultQuery
                    .Where(_baseFilter)
                    .Take(_dataCountPerPage).ToList());

                CarBrands = new CollectionWithSelection<CarBrand>(_ctx.CarBrand
                    .Include(c => c.CarModel)
                    .AsNoTracking()
                    .ToList());

                _filter.CarBrands = new CollectionWithSelection<CarBrand>(_ctx.CarBrand.AsNoTracking().Include(c => c.CarModel).ToList());
                _filter.CarTransmissions = new CollectionWithSelection<CarTransmission>(_ctx.CarTransmission.AsNoTracking().ToList());
                _filter.CarSteeringWheelPositions = new CollectionWithSelection<CarSteeringWheelPosition>(_ctx.CarSteeringWheelPosition.AsNoTracking().ToList());
                _filter.CarFuels = new CollectionWithSelection<CarFuel>(_ctx.CarFuel.AsNoTracking().ToList());;
                _filter.CarEngines = new CollectionWithSelection<CarEngine>(_ctx.CarEngine.AsNoTracking().ToList());
                _filter.CarDrives = new CollectionWithSelection<CarDrive>(_ctx.CarDrive.AsNoTracking().ToList());
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        #endregion Command implementation

        #region Other

        protected override IQueryable<CarEquipment> JoinBaseQuery(IQueryable<CarEquipment> query)
        {
            return query
                .Include(s => s.CarModel)
                .Include(s => s.CarTransmission)
                .Include(s => s.CarBody)
                .Include(s => s.CarSteeringWheelPosition)
                .Include(s => s.CarFuel)
                .Include(s => s.CarEngine)
                .Include(s => s.CarDrive)
                .AsNoTracking();
        }

        public sealed class FilterMaker : EntityFilterBase<CarEquipment>
        {
            #region Filter private fields

            private CollectionWithSelection<CarBody> _carBodies;
            private CollectionWithSelection<CarTransmission> _carTransmissions;
            private CollectionWithSelection<CarSteeringWheelPosition> _carSteeringWheelPositions;
            private CollectionWithSelection<CarFuel> _carFuels;
            private CollectionWithSelection<CarEngine> _carEngines;
            private CollectionWithSelection<CarDrive> _carDrives;
            private CollectionWithSelection<CarBrand> _carBrands;

            #endregion Filter private fields

            #region Filter properties

            public CollectionWithSelection<CarBody> CarBodies
            {
                get => _carBodies;
                set => SetProperty(ref _carBodies, value);
            }

            public CollectionWithSelection<CarTransmission> CarTransmissions
            {
                get => _carTransmissions;
                set => SetProperty(ref _carTransmissions, value);
            }

            public CollectionWithSelection<CarSteeringWheelPosition> CarSteeringWheelPositions
            {
                get => _carSteeringWheelPositions;
                set => SetProperty(ref _carSteeringWheelPositions, value);
            }

            public CollectionWithSelection<CarFuel> CarFuels
            {
                get => _carFuels;
                set => SetProperty(ref _carFuels, value);
            }

            public CollectionWithSelection<CarEngine> CarEngines
            {
                get => _carEngines;
                set => SetProperty(ref _carEngines, value);
            }

            public CollectionWithSelection<CarDrive> CarDrives
            {
                get => _carDrives;
                set => SetProperty(ref _carDrives, value);
            }

            public CollectionWithSelection<CarBrand> CarBrands
            {
                get => _carBrands;
                set => SetProperty(ref _carBrands, value);
            }

            public CarModel SelectedCarModel { get; set; }

            public string FullName { get; set; }

            public bool ShowIsDeleted { get; set; } = false;

            #endregion Filter properties

            public override Expression<Func<CarEquipment, bool>> MakeFilter()
            {
                var expression = defaultExpression;

                //if(_carModels.Selected != null)
                //{
                //    expression = expression.And(c => c.ModelID == true || c.IsDeleted == false);
                //}

                if (_carBrands.Selected != null && SelectedCarModel == null)
                {
                    expression = expression
                        .And(c => c.CarModel.CarBrandID == _carBrands.Selected.ID);
                }
                else if (SelectedCarModel != null)
                {
                    expression = expression
                        .And(c => c.ModelID == SelectedCarModel.ID);
                }

                if(_carDrives.Selected != null)
                {
                    expression = expression
                        .And(c => c.DriveID == _carDrives.Selected.ID);
                }

                if (_carBodies.Selected != null)
                {
                    expression = expression
                        .And(c => c.BodyID == _carBodies.Selected.ID);
                }

                if (_carEngines.Selected != null)
                {
                    expression = expression
                        .And(c => c.EngineID == _carEngines.Selected.ID);
                }

                if (_carFuels.Selected != null)
                {
                    expression = expression
                        .And(c => c.FuelID == _carFuels.Selected.ID);
                }

                if (_carSteeringWheelPositions.Selected != null)
                {
                    expression = expression
                        .And(c => c.SteeringWheelPositionID == _carSteeringWheelPositions.Selected.ID);
                }

                if (_carTransmissions.Selected != null)
                {
                    expression = expression
                        .And(c => c.TransmissionID == _carTransmissions.Selected.ID);
                }


                if (ShowIsDeleted == true)
                {
                    expression = expression.And(c => c.IsDeleted == true || c.IsDeleted == false);
                }

                return expression;
            }
        }

        #endregion Other
    }
}