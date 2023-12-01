using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using System.Windows.Input;

namespace SADA.Infastructure.Core
{
    public class DialogBase : ObservableObject
    {
        #region Constructor

        public DialogBase()
        {
            LoadedCommand = new RelayCommand<UserControl>(_loadedCommand);
        }

        #endregion Constructor

        #region Commands

        public RelayCommand<UserControl> LoadedCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _loadedCommand(UserControl uc)
        {
            if (uc == null) return;
            uc.Focusable = true;
            Keyboard.Focus(uc);
        }

        #endregion Commands implementations
    }
}