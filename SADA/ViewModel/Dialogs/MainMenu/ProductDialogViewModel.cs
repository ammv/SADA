using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.Start;

namespace SADA.ViewModel.Dialogs.MainMenu
{
    public class ProductDialogViewModel : DialogBase
    {
        private readonly ITabService _tabService;
        #region Constructor

        public ProductDialogViewModel(ITabService tabService = null) : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            ProductIncomeCommand = new RelayCommand(_ProductIncomeCommand);
            ProductSaleCommand = new RelayCommand(_ProductSaleCommand);
            PriceListCommand = new RelayCommand(_PriceListCommand);
            ProductRemainsCommand = new RelayCommand(_ProductRemainsCommand);
            _tabService = tabService;
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
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _ProductIncomeCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _ProductSaleCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _PriceListCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _ProductRemainsCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        #endregion Commands implementations
    }
}