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

        #endregion Fields

        #region Constructor

        public CarInSalonListViewModel(IDialogService dialogService, IWindowService windowService, ITabService tabService)
        {
            CloseCommand = new RelayCommand(_OnClose); 
            OpenCarFormCommand = new RelayCommand<FormMode>(_OpenCarFormCommand);

            _dialogService = dialogService;
            _windowService = windowService;
            _tabService = tabService;
        }
        protected CarInSalonListViewModel()
        { }

        #endregion Constructor

        #region Properties

        #endregion Properties

        #region Commands
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
            using (var ctx = new SADAEntities())
            {
                var query = _AttachFilters(ctx.Car);
                MaxPage = query.Count() / _dataCountPerPage;
                Entities = new ObservableCollection<DataLayer.Car>(await query.Skip((e.Info - 1) * _dataCountPerPage).Take(_dataCountPerPage).IncludeAll().ToListAsync());
            }
        }

        protected override void LoadedInner()
        {
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