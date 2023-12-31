﻿using CommunityToolkit.Mvvm.Input;
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

namespace SADA.ViewModel.MainMenu.Home.Expense
{
    public class CarExpenseViewModel : TabObservableObjectForm<CarExpense>
    {
        #region Fields

        private SADAEntities _ctx;

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        #region Main Form fields

        private IEnumerable<ExpenseGroup> _expenseGroups;
        private ObservableCollection<DataLayer.Car> _cars;
        private ExpenseGroup _selectedExpenseGroup;

        #endregion Main Form fields

        #endregion Fields

        #region Constructor

        public CarExpenseViewModel(IDialogService dialogService, ITabService tabService)
        {
            FormCommand = new RelayCommand(_FormCommand);
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

            _dialogService = dialogService;
            _tabService = tabService;

            FormModeTabNameMap = new Dictionary<FormMode, Func<CarExpense, string>>
            {
                {FormMode.Add, (s) => "Добавление расхода на автомобиль"},
                {FormMode.Edit, (s) => $"Изменение расхода на автомобиль №{s.ID}"},
                {FormMode.See, (s) => $"Просмотр расхода на автомобиль №{s.ID}"}
            };

            FormModeActionMap = new Dictionary<FormMode, Action>
            {
                {FormMode.Add,  () => _entity = new CarExpense{Expense = new DataLayer.Expense() } }
            };
        }

        protected CarExpenseViewModel()
        { }

        #endregion Constructor

        #region Properties

        #region Main Form properties

        public IEnumerable<ExpenseGroup> ExpenseGroups
        {
            get => _expenseGroups;
            set => SetProperty(ref _expenseGroups, value);
        }

        public ObservableCollection<DataLayer.Car> Cars
        {
            get => _cars;
            set => SetProperty(ref _cars, value);
        }

        public ExpenseGroup SelectedExpenseGroup
        {
            get => _selectedExpenseGroup;
            set => SetProperty(ref _selectedExpenseGroup, value);
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
                    _tabService.OpenTabForSelect<Car.Salon.CarInSalonListViewModel, DataLayer.Car>(
                        "Выбор автомобиля", _cars, (e) => Entity.Car = e);
                    break;
            }
        }

        private void _FormCommand()
        {
            try
            {
                if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.Add)
                {
                    string msg = $"Запись об расходе №{Entity.ID} на автомобиль изменена";
                    if (_currentFormMode == FormMode.Add)
                    {
                        _ctx.CarExpense.Add(Entity);
                        msg = "Новая запись об расходе на автомобиль добавлена";
                    }

                    _ctx.SaveChanges();

                    if(_currentFormMode ==  FormMode.Add)
                    {
                        Entity = new CarExpense();
                        Entity.Expense = new DataLayer.Expense();
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
                Entity = _ctx.CarExpense
                    .Include(c => c.Expense)
                    .Include(c => c.Expense.ExpenseType)
                    .Include(c => c.Expense.ExpenseType)
                    .Include(c => c.Expense.ExpenseType.ExpenseGroup)
                    .Include(c => c.Car.CarEquipment)
                    .Include(c => c.Car.CarEquipment.CarModel)
                    .Include(c => c.Car.CarEquipment.CarModel.CarBrand)
                    .FirstOrDefault(c => c.ID == Entity.ID);
            }
            else if (CurrentFormMode == FormMode.Add)
            {
                _ctx.CarExpense.Add(Entity);
            }

            if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.See)
            {
                SelectedExpenseGroup = Entity.Expense.ExpenseType.ExpenseGroup;
            }

            ExpenseGroups = _ctx.ExpenseGroup
                .OrderByDescending(c => c.Name)
                .ToList();

            Cars = new ObservableCollection<DataLayer.Car>(_ctx.Car
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