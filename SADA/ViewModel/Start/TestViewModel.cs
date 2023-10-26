using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SADA.Services;
using System.Windows.Input;

namespace SADA.ViewModel.Start
{
    class TestViewModel : ObservableObject
    {

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Commands

        public ICommand BackCommand { get; }

        private INavigationService _navigationService;

        #endregion

        #region Command implementations

        private void _BackCommand()
        {
            _navigationService.NavigateTo<ViewModel.Start.AuthViewModel>();
        }

        #endregion

        #region Конструктор
        public TestViewModel(INavigationService navigationService)
        {
            BackCommand = new RelayCommand(_BackCommand);
            _navigationService = navigationService;
        }

        #endregion
    }


}
