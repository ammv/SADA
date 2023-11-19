using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.ViewModel.Start;
using System;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    internal class ManualDialogViewModel : DialogBase
    {
        #region Constructor

        public ManualDialogViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
        }

        #endregion Constructor



        #region Commands

        public RelayCommand TestCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _TestCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = DateTime.Now.ToLongDateString();

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        #endregion Commands implementations
    }
}