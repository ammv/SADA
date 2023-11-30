using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.MainMenu.Home.Expense;
using SADA.ViewModel.Start;
using System;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    internal class ManualDialogViewModel : DialogBase
    {
        private readonly ITabService _tabService;
        #region Constructor

        public ManualDialogViewModel(ITabService tabService = null) : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            _tabService = tabService;
        }

        #endregion Constructor



        #region Commands

        public RelayCommand TestCommand { get; }
        public RelayCommand CarExpenseCommand { get; }
        public RelayCommand GeneralExpenseCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _TestCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }


        #endregion Commands implementations
    }
}