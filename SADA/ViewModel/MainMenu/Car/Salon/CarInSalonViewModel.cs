using CommunityToolkit.Mvvm.Input;
using SADA.Infastructure.Core;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    class CarInSalonViewModel: TabObservableObjectForm<DataLayer.Car>
    {
        #region Fields

        private readonly IDialogService _dialogService;
        private readonly ITabService _tabService;

        #endregion

        #region Constructor
        public CarInSalonViewModel(IDialogService dialogService, ITabService tabService)
        {
            CloseCommand = new RelayCommand(_OnClose);
            OpenTypeListCommand = new AsyncRelayCommand<Type>(_OpenTypeListCommand);

            _dialogService = dialogService;
            _tabService = tabService;
        }

        protected CarInSalonViewModel() { }

        #endregion

        #region Properties
        #endregion

        #region Commands

        public AsyncRelayCommand<Type> OpenTypeListCommand { get; }

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

        private async Task _OpenTypeListCommand(Type type)
        {
            switch(type.Name)
            {
                case nameof(DataLayer.Car):
                    break;
            }
        }

        #endregion

        #region Other
        protected override void LoadedInner()
        {
            
        }

        public override FormMode FormMode 
        { 
            get => _formMode;
            set
            {
                if(SetProperty(ref _formMode, value))
                {
                    switch(value)
                    {
                        case FormMode.Add:
                            Entity = new DataLayer.Car();
                            break;
                    }
                }
            }
        }

        #endregion

    }
}
