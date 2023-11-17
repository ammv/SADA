using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using SADA.Infastructure.Core;
using System;
using System.Windows;

namespace SADA.ViewModel.Start
{
    class WelcomeTestViewModel : TabObservableObject
    {

        public WelcomeTestViewModel()
        {
            CloseCommand = new RelayCommand(_OnClose);
        }

        private void _OnClose()
        {
            var result = HandyControl.Controls.MessageBox.Show($"Закрыть вкладку {Name}?", "Question", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                _RaiseCloseEvent();
            }   
        }
    }
}
