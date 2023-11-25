using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataLayer;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.Start;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    internal class CarInSalonListViewModel : TabObservableObjectList<DataLayer.Car>
    {
        #region Fields

        private readonly IDialogService _dialogService;
        private readonly IWindowService _windowService;
        private readonly ITabService _tabService;
        //private ObservableCollection<DataLayer.Car> _cars;
        //private int _dataCountPerPage = 20;
        //private int _maxPage = 0;
        //private DataLayer.Car _selectedCar = null;

        #endregion Fields

        #region Constructor

        public CarInSalonListViewModel(IDialogService dialogService, IWindowService windowService, ITabService tabService)
        {
            Guid guid = Guid.NewGuid();
            CloseCommand = new RelayCommand(_OnClose); 
            //PageUpdateCommand = new AsyncRelayCommand<HandyControl.Data.FunctionEventArgs<int>>(_PageUpdateCommand);
            OpenCarFormCommand = new RelayCommand<FormMode>(_OpenCarFormCommand);

            _dialogService = dialogService;
            _windowService = windowService;
            _tabService = tabService;
        }
        protected CarInSalonListViewModel()
        { }

        #endregion Constructor

        #region Properties

        //public ObservableCollection<DataLayer.Car> Cars
        //{
        //    get { return _cars; }
        //    set { SetProperty(ref _cars, value); }
        //}

        //public int DataCountPerPage
        //{
        //    get { return _dataCountPerPage; }
        //    set 
        //    { 
        //        if(SetProperty(ref _dataCountPerPage, value))
        //        {
        //            MaxPage = Cars.Count() / _dataCountPerPage + 1;
        //        }
        //    }
        //}

        //public int MaxPage
        //{
        //    get { return _maxPage; }
        //    set { SetProperty(ref _maxPage, value); }
        //}

        //public DataLayer.Car SelectedCar
        //{
        //    get { return _selectedCar; }
        //    set { SetProperty(ref _selectedCar, value); }
        //}

        #endregion Properties

        #region Commands

        //public AsyncRelayCommand<HandyControl.Data.FunctionEventArgs<int>> PageUpdateCommand { get; }
        public RelayCommand<FormMode> OpenCarFormCommand { get; }

        #endregion

        #region Command implementation

        private void _OnClose()
        {
            var result = _dialogService.ShowMessageBox("Вопрос", $"Закрыть вкладку {Name}?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _RaiseCloseEvent(); 
            }
        }

        private void _OpenCarFormCommand(FormMode parameter)
        {

            if (parameter == FormMode.Edit)
            {
                if(_selectedEntity == null)
                {
                    _dialogService.ShowMessageBox("Ошибка", "Вы не выбрали запись для редактирования", MessageBoxButton.OK);
                    return;
                }
                var vm = App.Current.GetService<CarInSalonViewModel>();
                vm.Name = "Изменение автомобиля";
                vm.Entity = SelectedEntity;
                vm.FormMode = FormMode.Edit;
                _tabService.OpenTab(vm);
            }
            else
            {
                var vm = App.Current.GetService<CarInSalonViewModel>();
                vm.Name = "Добавление автомобиля";
                vm.FormMode = FormMode.Add;
                _tabService.OpenTab(vm);
            }
        }

        protected override async Task _PageUpdateCommand(HandyControl.Data.FunctionEventArgs<int> e)
        {
            using (var ctx = new SADAEntities())
            {
                var query = _AttachFilters(ctx.Car);
                MaxPage = query.Count() / _dataCountPerPage;
                Entities = new ObservableCollection<DataLayer.Car>(await query.Skip((e.Info - 1) * _dataCountPerPage).Take(_dataCountPerPage).IncludeAll().ToListAsync());
            }
        }

        protected override void LoadedInner()
        {
            Thread.Sleep(3000);
            using (var ctx = new SADAEntities())
            {
                Entities = new ObservableCollection<DataLayer.Car>(ctx.Car.IncludeAll().ToList());
                for (int i = 0; i < 100; i++)
                {
                    Entities.Add(Entities[i]);
                }
                MaxPage = Entities.Count() / _dataCountPerPage + 1;
            }
        }

        #endregion Command implementation

        #region Other

        private IQueryable<T> _AttachFilters<T>(DbSet<T> dbset) where T: class
        {
            return dbset.AsQueryable<T>();
        }

        #endregion Other
    }
}