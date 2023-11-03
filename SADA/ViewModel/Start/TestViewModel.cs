using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace SADA.ViewModel.Start
{
    class TestViewModel : ObservableObject
    {
        private string _buttonText;

        public TestViewModel()
        {
            ButtonText = DateTime.Now.ToLongTimeString();
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set { SetProperty(ref _buttonText, value); }
        }

    }
}
