using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.ViewModel.Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    class SalaryAndStaffViewModel : DialogBase
    {
        #region Fields
        #endregion

        #region Constructor

        public SalaryAndStaffViewModel(): base()
        {
            TestCommand = new RelayCommand(_TestCommand);
        }

        #endregion

        #region Properties
        #endregion

        #region Commands

        public RelayCommand TestCommand { get; }

        #endregion

        #region Commands implementations

        private void _TestCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = DateTime.Now.ToLongDateString();

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        #endregion

        #region Other
        #endregion
    }
}
