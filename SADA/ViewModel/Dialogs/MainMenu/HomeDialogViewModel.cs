using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.Start;
using System;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    internal class HomeDialogViewModel : DialogBase
    {
        private readonly ITabService _tabService;
        #region Constructor

        public HomeDialogViewModel(ITabService tabService = null) : base()
        {
            TestCommand = new RelayCommand(_TestCommand);

            CounteragentCommand = new RelayCommand(_CounteragentCommand);
            InteractionWithCounteragentCommand = new RelayCommand(_InteractionWithCounteragentCommand);

            CarExpenseCommand = new RelayCommand(_CarExpenseCommand);
            GeneralExpenseCommand = new RelayCommand(_GeneralExpenseCommand);

            CarSaleIncomeCommand = new RelayCommand(_CarSaleIncomeCommand);
            ProductSaleIncomeCommand = new RelayCommand(_ProductSaleIncomeCommand);
            _tabService = tabService;
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
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _CounteragentCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _InteractionWithCounteragentCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _CarExpenseCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _GeneralExpenseCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _CarSaleIncomeCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _ProductSaleIncomeCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        #endregion Commands implementations
    }
}