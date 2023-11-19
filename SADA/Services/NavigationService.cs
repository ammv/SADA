using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace SADA.Services
{
    public class NavigationService : ObservableObject, INavigationService
    {
        private ObservableObject _currentView;
        private readonly IServiceProvider _provider;

        public ObservableObject CurrentView
        {
            get => _currentView;
            private set
            {
                SetProperty(ref _currentView, value);
            }
        }

        public NavigationService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void NavigateTo<T>() where T : ObservableObject
        {
            var view = _provider.GetService(typeof(T)) as ObservableObject;
            CurrentView = view;
        }
    }
}