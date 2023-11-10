using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SADA.Infastructure.Core
{
    class DialogBase: ObservableObject
    {
        #region Fields
        #endregion

        #region Constructor

        public DialogBase()
        {
            LoadedCommand = new RelayCommand<UserControl>(_loadedCommand);
        }

        #endregion

        #region Properties
        #endregion

        #region Commands

        public RelayCommand<UserControl> LoadedCommand { get; }

        #endregion

        #region Commands implementations

        private void _loadedCommand(UserControl uc)
        {
            if (uc == null) return;
            uc.Focusable = true;
            Keyboard.Focus(uc);
        }

        #endregion

        #region Other
        #endregion
    }
}
