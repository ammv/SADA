using CommunityToolkit.Mvvm.Input;
using DataLayer;
using SADA.Infastructure.Core;
using SADA.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    internal class CarInSalonViewModel : TabObservableObject
    {
        #region Fields

        private readonly IDialogService _dialogService;
        private ObservableCollection<DataLayer.Car> _cars;

        private bool _isLoading = true;
        private bool _isLoaded = false;

        #endregion Fields

        #region Constructor

        public CarInSalonViewModel(IDialogService dialogService)
        {
            CloseCommand = new RelayCommand(_OnClose);
            LoadedCommand = new AsyncRelayCommand(() => Task.Run(_LoadedCommand));
            _dialogService = dialogService;   
        }

        protected CarInSalonViewModel()
        { }

        #endregion Constructor

        #region Properties

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public ObservableCollection<DataLayer.Car> Cars
        {
            get { return _cars; }
            set { SetProperty(ref _cars, value); }
        }

        public string Test
        {
            get { return "Test string"; }
        }


        #endregion Properties

        #region Commands

        public AsyncRelayCommand LoadedCommand { get; }

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

        private void _LoadedCommand()
        {
            if(_isLoaded)
            {
                return;
            }
            IsLoading = true;

            using (var ctx = new SADAEntities())
            {
                Cars = new ObservableCollection<DataLayer.Car>(ctx.Car.IncludeAll().ToList());
                Thread.Sleep(4000);
            }

            IsLoading = false;
            _isLoaded = true;


        }

        #endregion Command implementation

        #region Other

        ~CarInSalonViewModel()
        {
            MessageBox.Show($"{nameof(CarInSalonViewModel)} has cleared from RAM");
        }

        #endregion Other
    }
}