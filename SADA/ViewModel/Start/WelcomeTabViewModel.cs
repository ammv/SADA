using CommunityToolkit.Mvvm.Input;
using SADA.Infastructure.Core;
using System.Windows;

namespace SADA.ViewModel.Start
{
    public class WelcomeTabViewModel : TabObservableObject
    {
        public WelcomeTabViewModel()
        {
            Name = "Приветствие";
        }

        protected override void _OnClose()
        {
            var result = HandyControl.Controls.MessageBox.Show($"Закрыть вкладку {Name}?", "Question", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _RaiseCloseEvent();
            }
        }
    }
}