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

        public SalaryAndStaffViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            AccrualOfSalariesCommand = new RelayCommand(_AccrualOfSalariesCommand);
            SalaryReportCommand = new RelayCommand(_SalaryReportCommand);
            StaffCommand = new RelayCommand(_StaffCommand);
        }

        #endregion

        #region Properties
        #endregion

        #region Commands

        public RelayCommand TestCommand { get; }
        public RelayCommand AccrualOfSalariesCommand { get; }
        public RelayCommand SalaryReportCommand { get; }
        public RelayCommand StaffCommand { get; }

        #endregion

        #region Commands implementations

        private void _TestCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = DateTime.Now.ToLongDateString();

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _AccrualOfSalariesCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_AccrualOfSalariesCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _SalaryReportCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_SalaryReportCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _StaffCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_StaffCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        #endregion

        #region Other
        #endregion
    }
}
