using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.ViewModel.Start;
using System;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    internal class SalaryAndStaffDialogViewModel : DialogBase
    {
        #region Constructor

        public SalaryAndStaffDialogViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            AccrualOfSalariesCommand = new RelayCommand(_AccrualOfSalariesCommand);
            SalaryReportCommand = new RelayCommand(_SalaryReportCommand);
            StaffCommand = new RelayCommand(_StaffCommand);
        }

        #endregion Constructor



        #region Commands

        public RelayCommand TestCommand { get; }
        public RelayCommand AccrualOfSalariesCommand { get; }
        public RelayCommand SalaryReportCommand { get; }
        public RelayCommand StaffCommand { get; }

        #endregion Commands

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

        #endregion Commands implementations
    }
}