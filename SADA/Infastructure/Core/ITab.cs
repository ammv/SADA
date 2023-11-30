using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SADA.Infastructure.Core
{
    public interface ITab
    {
        string Name { get; set; }
        SolidColorBrush BottomBrush { get; set; }
        ICommand CloseCommand { get; }
        RelayCommand<FrameworkElement> ChangeBottomBrushCommand { get; }

        event EventHandler CloseRequested;

        object ViewModel { get; }
    }
}