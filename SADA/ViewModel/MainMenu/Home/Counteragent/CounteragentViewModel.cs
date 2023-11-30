﻿using CommunityToolkit.Mvvm.Input;
using DataLayer;
using DataLayer.Helpers;
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

namespace SADA.ViewModel.MainMenu.Home.Counteragent
{
    public class CounteragentViewModel : TabObservableObjectForm<DataLayer.Counteragent>
    {
        #region Fields

        private SADAEntities _ctx;

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        #region Main Form fields

        private IEnumerable<CounteragentGroup> _counteragentGroups;
        private IEnumerable<CounteragentType> _counteragentTypes;
        private IEnumerable<TaxationSystemType> _taxationSystemTypes;

        private ObservableCollection<CounteragentContactPerson> _counteragentContactPersons;

        private CounteragentType _selectedCounteragentType;
        private CounteragentContactPerson _selectedCounteragentContactPerson;

        private JuridicalPerson _juridicalPerson;
        private SoloTrader _soloTrader;
        private IndividualPerson _individualPerson;

        #endregion Main Form fields

        #endregion Fields

        #region Constructor

        public CounteragentViewModel(IDialogService dialogService, ITabService tabService)
        {
            CloseCommand = new RelayCommand(_OnClose);
            FormCommand = new RelayCommand(_FormCommand);
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

            _dialogService = dialogService;
            _tabService = tabService;
        }

        protected CounteragentViewModel()
        { }

        #endregion Constructor

        #region Properties

        #region Main Form properties

        public IEnumerable<CounteragentGroup> CounteragentGroups
        {
            get => _counteragentGroups;
            set => SetProperty(ref _counteragentGroups, value);
        }

        public IEnumerable<CounteragentType> CounteragentTypes
        {
            get => _counteragentTypes;
            set => SetProperty(ref _counteragentTypes, value);
        }

        public IEnumerable<TaxationSystemType> TaxationSystemTypes
        {
            get => _taxationSystemTypes;
            set => SetProperty(ref _taxationSystemTypes, value);
        }

        public ObservableCollection<CounteragentContactPerson> CounteragentContactPersons
        {
            get => _counteragentContactPersons;
            set => SetProperty(ref _counteragentContactPersons, value);
        }

        public CounteragentContactPerson SelectedCounteragentContactPerson
        {
            get => _selectedCounteragentContactPerson;
            set => SetProperty(ref _selectedCounteragentContactPerson, value);
        }

        public CounteragentType SelectedCounteragentType
        {
            get => _selectedCounteragentType;
            set
            { 
                if (SetProperty(ref _selectedCounteragentType, value))
                {
                    Entity.CounteragentType = _selectedCounteragentType;
                    OnPropertyChanged(nameof(SelectedCounteragentTypeNative));
                }
            }
        }

        public Type SelectedCounteragentTypeNative
        {
            get
            {
                if (Entity.CounteragentType == null) return null;
                switch(CounteragentTypeHelper.CounteragentTypeMap[Entity.CounteragentType.Name])
                {
                    case CounteragentTypeEnum.IndividualPerson:
                        return typeof(IndividualPerson);
                    case CounteragentTypeEnum.SoloTrader:
                        return typeof(SoloTrader);
                    case CounteragentTypeEnum.JuridicalPerson:
                        return typeof(JuridicalPerson);
                    default:
                        return null;
                }
            }
        }

        public JuridicalPerson JuridicalPerson
        {
            get => _juridicalPerson;
            set => SetProperty(ref _juridicalPerson, value);
        }

        public SoloTrader SoloTrader
        {
            get => _soloTrader;
            set => SetProperty(ref _soloTrader, value);
        }

        public IndividualPerson IndividualPerson
        {
            get => _individualPerson;
            set => SetProperty(ref _individualPerson, value);
        }


        #endregion Main Form properties

        #endregion Properties

        #region Commands

        public AsyncRelayCommand<Type> OpenTypeListCommand { get; }
        public RelayCommand FormCommand { get; }

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

        private async Task _OpenTypeListCommand(Type type)
        {
            switch (type.Name)
            {
                //case nameof(DataLayer.Car):
                //    var vm = App.Current.GetService<Salon.CarInSalonListViewModel>();
                //    vm.Name = "Выбор автомобиля";
                //    vm.CurrentListMode = ListMode.Select;
                //    vm.SelectAction = (car) =>
                //    {
                //        Entity.Car = _cars.FirstOrDefault(c => c.ID == car.ID);
                //    };
                //    _tabService.OpenTab(vm);
                //    break;
            }
            //OnPropertyChanged()
        }

        private void _FormCommand()
        {
            try
            {
                if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.Add)
                {
                    string msg = $"Запись об общем расходе изменена №{Entity.ID}";
                    if (_currentFormMode == FormMode.Add)
                    {
                        _ctx.Counteragent.Add(Entity);
                        msg = "Новая запись об общем расходе добавлена";
                    }

                    Entity.CounteragentContactPerson = _counteragentContactPersons;

                    _ctx.SaveChanges();

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
                Entity = _ctx.Counteragent
                    .Include(c => c.JuridicalPerson)
                    .Include(c => c.JuridicalPerson.Address)
                    .Include(c => c.JuridicalPerson.TaxationSystemType)
                    .Include(c => c.SoloTrader)
                    .Include(c => c.SoloTrader.Address)
                    .Include(c => c.SoloTrader.TaxationSystemType)
                    .Include(c => c.IndividualPerson)
                    .Include(c => c.CounteragentContactPerson)
                    .Include(c => c.CounteragentType)
                    .Include(c => c.CounteragentGroup)
                    .FirstOrDefault(c => c.ID == Entity.ID);
            }
            else if (CurrentFormMode == FormMode.Add)
            {
                _ctx.Counteragent.Add(Entity);
            }

            if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.See)
            {
                
                CounteragentContactPersons = new ObservableCollection<CounteragentContactPerson>(Entity.CounteragentContactPerson);
                SelectedCounteragentType = Entity.CounteragentType;
                JuridicalPerson = Entity.JuridicalPerson;
                SoloTrader = Entity.SoloTrader;
                IndividualPerson = Entity.IndividualPerson;
            }

            CounteragentGroups = _ctx.CounteragentGroup
                .OrderByDescending(c => c.Name)
                .ToList();
            TaxationSystemTypes = _ctx.TaxationSystemType
                .OrderByDescending(c => c.Name)
                .ToList();
            CounteragentTypes = _ctx.CounteragentType
                .OrderByDescending(c => c.Name)
                .ToList();

            

            //ExpenseGroups = _ctx.CounteragentGroup
            //    .OrderByDescending(c => c.Name)
            //    .ToList();

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
                            _entity = new DataLayer.Counteragent();
                            break;
                    }
                }
            }
        }

        #endregion Other
    }
}