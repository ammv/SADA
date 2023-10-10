using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SADA.Services
{
    public class NavigationService : ObservableObject, INavigationService
    {
        private Page _currentView;
        private readonly IServiceProvider _provider;

        public Page CurrentView
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

        public void NavigateTo<T>() where T : Page
        {
            var view = _provider.GetService(typeof(T)) as Page;
            CurrentView = view;
        }
    }
}
