using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.ViewModel.MainMenu.Car.Salon;
using SADA.ViewModel.Start;
using System;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    internal class CarDialogViewModel : DialogBase
    {
        #region Constructor

        public CarDialogViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            PayToCounteragentCommand = new RelayCommand(_PayToCounteragentCommand);
            PurchaseFromCounteragentCommand = new RelayCommand(_PurchaseFromCounteragentCommand);
            CarInSalonCommand = new RelayCommand(_CarInSalonCommand);
            EquipmentCommand = new RelayCommand(_EquipmentCommand);
        }

        #endregion Constructor



        #region Commands

        public RelayCommand TestCommand { get; }
        public RelayCommand PayToCounteragentCommand { get; }
        public RelayCommand PurchaseFromCounteragentCommand { get; }

        public RelayCommand CarInSalonCommand { get; }

        public RelayCommand EquipmentCommand { get; }

        #endregion Commands

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
            var vm = App.Current.GetService<CarInSalonViewModel>();

            vm.Name = "Автомобили в салоне";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(vm));
        }

        private void _EquipmentCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_EquipmentCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        #endregion Commands implementations
    }
}