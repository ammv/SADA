using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SADA.ViewModel.Start
{
    public class AuthViewModel : ObservableObject
    {
        #region Fields

        private INavigationService _navigationService;



        #endregion

        #region Properties

        #endregion

        #region Commands

        public ICommand AuthCommand { get; }

        #endregion

        #region Command implementations

        private void _AuthCommand()
        {
            Task.Run(() => WeakReferenceMessenger.Default.Send("Hello!"));
            _navigationService.NavigateTo<TestViewModel>();
        }

        #endregion

        public AuthViewModel(INavigationService navigationService)
        {
            AuthCommand = new RelayCommand(_AuthCommand);
            _navigationService = navigationService;
        }


    }
}
