using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using SADA.Infastructure.Core;
using System;
using System.Windows;

namespace SADA.ViewModel.Start
{
    class TestViewModel : TabObservableObject
    {
        private string _buttonText;

        public TestViewModel()
        {
            ButtonText = DateTime.Now.ToLongTimeString();
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

        public string ButtonText
        {
            get { return _buttonText; }
            set { SetProperty(ref _buttonText, value); }
        }
    }
}
