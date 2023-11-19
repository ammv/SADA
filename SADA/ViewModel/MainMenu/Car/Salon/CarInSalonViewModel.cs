using CommunityToolkit.Mvvm.Input;
using SADA.Infastructure.Core;
using SADA.Services;
using System.Windows;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    internal class CarInSalonViewModel: TabObservableObject
    {
        private readonly IDialogService _dialogService;

        public CarInSalonViewModel(IDialogService dialogService)
        {
            CloseCommand = new RelayCommand(_OnClose);
            _dialogService = dialogService;
        }

        protected CarInSalonViewModel() {  }

        private void _OnClose()
        {
            var result = _dialogService.ShowMessageBox("Вопрос", $"Закрыть вкладку {Name}?",  MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _RaiseCloseEvent();
            }
        }
    }
}