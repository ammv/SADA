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
    class CarDialogViewModel : DialogBase
    {
        #region Fields
        #endregion

        #region Constructor

        public CarDialogViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            PayToCounteragentCommand = new RelayCommand(_PayToCounteragentCommand);
            PurchaseFromCounteragentCommand = new RelayCommand(_PurchaseFromCounteragentCommand);
            CarInSalonCommand = new RelayCommand(_CarInSalonCommand);
            EquipmentCommand = new RelayCommand(_EquipmentCommand);
        }

        #endregion

        #region Properties
        #endregion

        #region Commands

        public RelayCommand TestCommand { get; }
        public RelayCommand PayToCounteragentCommand { get; }
        public RelayCommand PurchaseFromCounteragentCommand { get; }

        public RelayCommand CarInSalonCommand { get; }

        public RelayCommand EquipmentCommand { get; }



        #endregion

        #region Commands implementations

        private void _TestCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = DateTime.Now.ToLongDateString();

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _PayToCounteragentCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "Оплата контрагентам";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _PurchaseFromCounteragentCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "PurchaseFromCounteragentCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _CarInSalonCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_CarInSalonCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _EquipmentCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_EquipmentCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        #endregion

        #region Other
        #endregion
    }
}
