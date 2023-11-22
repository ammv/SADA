using System;
using System.Windows.Input;

namespace SADA.Infastructure.Core
{
    public interface ITab
    {
        string Name { get; set; }
        ICommand CloseCommand { get; }

        event EventHandler CloseRequested;

        object ViewModel { get; }
    }
}