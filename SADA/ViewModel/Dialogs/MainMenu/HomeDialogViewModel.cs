using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.ViewModel.Start;
using System;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    internal class HomeDialogViewModel : DialogBase
    {
        #region Constructor

        public HomeDialogViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);

            CounteragentCommand = new RelayCommand(_CounteragentCommand);
            InteractionWithCounteragentCommand = new RelayCommand(_InteractionWithCounteragentCommand);

            CarExpenseCommand = new RelayCommand(_CarExpenseCommand);
            GeneralExpenseCommand = new RelayCommand(_GeneralExpenseCommand);

            CarSaleIncomeCommand = new RelayCommand(_CarSaleIncomeCommand);
            ProductSaleIncomeCommand = new RelayCommand(_ProductSaleIncomeCommand);
        }

        #endregion Constructor



        #region Commands

        public RelayCommand TestCommand { get; }

        public RelayCommand CounteragentCommand { get; }
        public RelayCommand InteractionWithCounteragentCommand { get; }

        public RelayCommand CarExpenseCommand { get; }
        public RelayCommand GeneralExpenseCommand { get; }

        public RelayCommand CarSaleIncomeCommand { get; }
        public RelayCommand ProductSaleIncomeCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _TestCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = DateTime.Now.ToLongDateString();

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _CounteragentCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_CounteragentCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _InteractionWithCounteragentCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_InteractionWithCounteragentCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _CarExpenseCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_CarExpenseCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _GeneralExpenseCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_GeneralExpenseCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _CarSaleIncomeCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_CarSaleIncomeCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _ProductSaleIncomeCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_ProductSaleIncomeCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        #endregion Commands implementations
    }
}