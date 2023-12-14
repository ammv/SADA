using CommunityToolkit.Mvvm.Input;
using DataLayer;
using HandyControl.Controls;
using SADA.Helpers;
using SADA.Infastructure.Converters;
using SADA.Infastructure.Core;
using SADA.Infastructure.Core.Enums;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Window = System.Windows.Window;

namespace SADA.ViewModel.MainMenu.SalaryAndStaff.Staff
{
    public class StaffViewModel : TabObservableObjectForm<DataLayer.Staff>
    {
        #region Fields

        private SADAEntities _ctx;

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        private StaffToStringConverter _staffToStringConverter = new StaffToStringConverter();

        #region Main Form fields

        private IEnumerable<PassportGiver> _passportGivers;
        private IEnumerable<StaffRole> _staffRoles;
        private IEnumerable<StaffPost> _staffPosts;
        private IEnumerable<User> _users;
        private IEnumerable<CarDealership> _carDealerships;

        private Uri _uriFile;


        #endregion Main Form fields

        #endregion Fields

        #region Constructor

        public StaffViewModel(IDialogService dialogService, ITabService tabService)
        {;
            FormCommand = new RelayCommand(_FormCommand);
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);
            OpenImageBrowserCommand = new RelayCommand(_OpenImageBrowserCommand);

            AddImageCommand = new RelayCommand(_AddImageCommand);
            DeleteImageCommand = new RelayCommand(_DeleteImageCommand);

            _dialogService = dialogService;
            _tabService = tabService;

            FormModeTabNameMap = new Dictionary<FormMode, Func<DataLayer.Staff, string>>
            {
                {FormMode.Add, (s) => "Добавление сотрудника"},
                {FormMode.Edit, (s) => $"Изменение сотрудника {_staffToStringConverter.Convert(s, null, null, null)} №{s.ID}"},
                {FormMode.See, (s) => $"Просмотр сотрудника {_staffToStringConverter.Convert(s, null, null, null)} №{s.ID}"},
            };

            FormModeActionMap = new Dictionary<FormMode, Action>
            {
                {FormMode.Add,  () => _entity = new DataLayer.Staff {IsDeleted = false, Passport = new Passport()} }
            };
        }

        protected StaffViewModel()
        { }

        #endregion Constructor

        #region Properties

        #region Main Form properties

        public Uri UriFile
        {
            get => _uriFile;
            set => SetProperty(ref _uriFile, value);
        }

        public IEnumerable<PassportGiver> PassportGivers
        {
            get => _passportGivers;
            set => SetProperty(ref _passportGivers, value);
        }

        public IEnumerable<StaffRole> StaffRoles
        {
            get => _staffRoles;
            set => SetProperty(ref _staffRoles, value);
        }

        public IEnumerable<StaffPost> StaffPosts
        {
            get => _staffPosts;
            set => SetProperty(ref _staffPosts, value);
        }

        public IEnumerable<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public IEnumerable<CarDealership> CarDealerships
        {
            get => _carDealerships;
            set => SetProperty(ref _carDealerships, value);
        }

        #endregion Main Form properties

        #endregion Properties

        #region Commands

        public AsyncRelayCommand<Type> OpenTypeListCommand { get; }
        public RelayCommand FormCommand { get; }

        public RelayCommand AddImageCommand { get; }

        public RelayCommand DeleteImageCommand { get; }
        public RelayCommand OpenImageBrowserCommand { get; }

        #endregion Commands

        #region Command implementation

        private void _OpenImageBrowserCommand()
        {
        }

        private void _AddImageCommand()
        {
            var imageFiles = _dialogService.ShowFileDialog(_dialogService.FileDialogImageFilter);

            try
            {
                if (imageFiles == null) return;
                if (Entity.File == null)
                {
                    Entity.File = new DataLayer.File
                    {
                        FileID = Guid.NewGuid()
                    };
                } 

                Entity.File.Data = System.IO.File.ReadAllBytes(imageFiles[0].FullName);
            }
            catch (Exception ex)
            {

                _dialogService.ShowMessageBox("Ошибка во чтения файла", ex.Message + ex.InnerException?.Message, MessageBoxButton.OK);
            }

            
        }

        private void _DeleteImageCommand()
        {
            if(Entity.File != null)
            Entity.File.Data = null;
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
                    string msg = $"Запись об сотруднике №{Entity.ID} изменена";
                    if (_currentFormMode == FormMode.Add)
                    {
                        _ctx.Staff.Add(Entity);
                        msg = "Новая запись об сотруднике добавлена";
                    }

                    _ctx.SaveChanges();

                    if(_currentFormMode == FormMode.Add)
                    {
                        Entity = new DataLayer.Staff
                        {
                            IsDeleted = false
                        };
                        Entity.Passport = new Passport();
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
                Entity = _ctx.Staff
                    .Include(s => s.User)
                    .Include(s => s.File)
                    .Include(s => s.StaffRole)
                    .Include(s => s.StaffPost)
                    .Include(s => s.CarDealership)
                    .Include(s => s.Passport)
                    .Include(s => s.Passport.PassportGiver)
                    .FirstOrDefault(c => c.ID == Entity.ID);
            }
            else if (CurrentFormMode == FormMode.Add)
            {
                //_ctx.Expense.Add(Entity);
            }

            if (_currentFormMode == FormMode.Edit || _currentFormMode == FormMode.See)
            {
               // SelectedExpenseGroup = Entity.ExpenseType.ExpenseGroup;
            }

            Users = _ctx.User
                    .ToList();

            CarDealerships = _ctx.CarDealership
                .ToList();

            StaffRoles = _ctx.StaffRole
                .ToList();

            StaffPosts = _ctx.StaffPost
                .ToList();

            PassportGivers = _ctx.PassportGiver
                .ToList();

            // Wait EF loading data
            Thread.Sleep(100);
        }

        #endregion Other
    }
}