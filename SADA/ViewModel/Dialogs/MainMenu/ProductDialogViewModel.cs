using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.ViewModel.Start;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    internal class ProductDialogViewModel : DialogBase
    {
        #region Constructor

        public ProductDialogViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            ProductIncomeCommand = new RelayCommand(_ProductIncomeCommand);
            ProductSaleCommand = new RelayCommand(_ProductSaleCommand);
            PriceListCommand = new RelayCommand(_PriceListCommand);
            ProductRemainsCommand = new RelayCommand(_ProductRemainsCommand);
        }

        #endregion Constructor



        #region Commands

        public RelayCommand TestCommand { get; }

        public RelayCommand ProductIncomeCommand { get; }

        public RelayCommand ProductSaleCommand { get; }

        public RelayCommand PriceListCommand { get; }
        public RelayCommand ProductRemainsCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _TestCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_TestCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _ProductIncomeCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_ProductIncomeCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _ProductSaleCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_ProductSaleCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _PriceListCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_PriceListCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _ProductRemainsCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "_ProductRemainsCommand";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        #endregion Commands implementations
    }
}