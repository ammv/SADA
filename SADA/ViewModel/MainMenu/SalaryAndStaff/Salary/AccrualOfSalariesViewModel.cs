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

namespace SADA.ViewModel.MainMenu.SalaryAndStaff.Salary
{
    public class AccrualOfSalariesViewModel : TabObservableObjectForm<DataLayer.Salary>
    {
        #region Fields

        private SADAEntities _ctx;

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        #region Main Form fields

        private ObservableCollection<DataLayer.Car> _cars;
        private IEnumerable<DataLayer.Staff> _staffs;
        private IEnumerable<SalaryType> _salaryTypes;

        private ExpenseGroup _selectedExpenseGroup;

        //private CollectionWithSelection<DataLayer.Staff> _staffs;
        //private CollectionWithSelection<SalaryType> _salaryTypes;
        //private CollectionWithSelection<DataLayer.Car> _cars;

        #endregion Main Form fields

        #endregion Fields

        #region Constructor

        public AccrualOfSalariesViewModel(IDialogService dialogService, ITabService tabService)
        {
            FormCommand = new RelayCommand(_FormCommand);
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

            _dialogService = dialogService;
            _tabService = tabService;
        }

        protected AccrualOfSalariesViewModel()
        { }

        #endregion Constructor

        #region Properties

        #region Main Form properties

        public ObservableCollection<DataLayer.Car> Cars
        {
            get => _cars;
            set => SetProperty(ref _cars, value);
        }

        public IEnumerable<DataLayer.Staff> Staffs
        {
            get => _staffs;
            set => SetProperty(ref _staffs, value);
        }

        public IEnumerable<SalaryType> SalaryTypes
        {
            get => _salaryTypes;
            set => SetProperty(ref _salaryTypes, value);
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
                    string msg = $"Запись об начислнии зарплаты №{Entity.ID}";
                    if (_currentFormMode == FormMode.Add)
                    {
                        _ctx.Salary.Add(Entity);
                        msg = "Новая запись об начислении зарплаты добавлена";
                    }

                    _ctx.SaveChanges();

                    if(_currentFormMode == FormMode.Add)
                    {
                        _entity = new DataLayer.Salary();
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
                Entity = _ctx.Salary
                    .Include(s => s.Staff)
                    .Include(s => s.SalaryType)
                    .Include(s => s.Car)
                    .FirstOrDefault(c => c.ID == Entity.ID);
            }
            else if (CurrentFormMode == FormMode.Add)
            {
                _ctx.Salary.Add(Entity);
            }

            if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.See)
            {
               // SelectedExpenseGroup = Entity.ExpenseType.ExpenseGroup;
            }

            Cars = new ObservableCollection<DataLayer.Car>(_ctx.Car
                .Include(c => c.CarEquipment)
                .Include(c => c.CarEquipment.CarModel)
                .Include(c => c.CarEquipment.CarModel.CarBrand)
                .ToList());

            SalaryTypes = _ctx.SalaryType
                  .ToList();


            Staffs = _ctx.Staff
            .Include(s => s.StaffRole)
            .Include(s => s.StaffPost)
            .Include(s => s.Passport)
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
                            _entity = new DataLayer.Salary();
                            break;
                    }
                }
            }
        }

        #endregion Other
    }
}