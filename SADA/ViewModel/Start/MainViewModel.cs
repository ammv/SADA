using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.Start
{
    class MainViewModel: ObservableObject, IRecipient<string>
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
            NavigationService.NavigateTo<View.Start.AuthView>();
        }

        public void Receive(string message)
        {
            MessageBox.Show(message);
        }
    }
}
