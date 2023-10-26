using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Services;
using System.Windows;

namespace SADA.ViewModel.Start
{
    class MainViewModel : ObservableObject, IRecipient<string>
    {
        private INavigationService _navigationService;

        public INavigationService NavigationService
        {
            get { return _navigationService; }
            set { _navigationService = value; }
        }

        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigationService.NavigateTo<ViewModel.Start.AuthViewModel>();
        }

        public void Receive(string message)
        {
            MessageBox.Show(message);
        }
    }
}
