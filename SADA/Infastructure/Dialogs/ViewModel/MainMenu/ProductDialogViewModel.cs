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
    class ProductDialogViewModel : DialogBase
    {
        #region Fields
        #endregion

        #region Constructor

        public ProductDialogViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            ProductIncomeCommand = new RelayCommand(_ProductIncomeCommand);
            ProductSaleCommand = new RelayCommand(_ProductSaleCommand);
            PriceListCommand = new RelayCommand(_PriceListCommand);
            ProductRemainsCommand = new RelayCommand(_ProductRemainsCommand);
        }

        #endregion

        #region Properties
        #endregion

        #region Commands

        public RelayCommand TestCommand { get; }

        public RelayCommand ProductIncomeCommand { get; }

        public RelayCommand ProductSaleCommand { get; }

        public RelayCommand PriceListCommand { get; }
        public RelayCommand ProductRemainsCommand { get; }

        #endregion

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

        #endregion

        #region Other
        #endregion
    }
}
