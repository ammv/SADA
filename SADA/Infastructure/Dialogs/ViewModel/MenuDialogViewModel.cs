using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using SADA.Infastructure.Messages;
using SADA.ViewModel.Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SADA.Infastructure.Dialogs.ViewModel
{
    class MenuDialogViewModel: ObservableObject
    {
        #region Fields
        #endregion

        #region Constructor

        public MenuDialogViewModel()
        {
            LoadedCommand = new RelayCommand<UserControl>(_loadedCommand);
            TestCommand = new RelayCommand(_TestCommand);
        }

        #endregion

        #region Properties
        #endregion

        #region Commands

        public RelayCommand<UserControl> LoadedCommand { get; }
        public RelayCommand TestCommand { get; }

        #endregion

        #region Commands implementations

        private void _loadedCommand(UserControl uc)
        {
            if (uc == null) return;
            uc.Focusable = true;
            Keyboard.Focus(uc);
        }
        private void _TestCommand()
        {
            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(App.Current.GetService<TestViewModel>()));
        }

        #endregion

        #region Other
        #endregion
    }
}
