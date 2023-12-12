using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.MainMenu.Home.Counteragent;
using SADA.ViewModel.MainMenu.Home.Expense;
using SADA.ViewModel.Start;
using System;

namespace SADA.ViewModel.Dialogs.MainMenu
{
    public class HomeDialogViewModel : DialogBase
    {
        #region Constructor

        public HomeDialogViewModel(ITabService tabService = null) : base(tabService)
        {
            //TestCommand = new RelayCommand(_TestCommand);

            //CounteragentCommand = new RelayCommand(_CounteragentCommand);
            //InteractionWithCounteragentCommand = new RelayCommand(_InteractionWithCounteragentCommand);

            //CarExpenseCommand = new RelayCommand(_CarExpenseCommand);
            //GeneralExpenseCommand = new RelayCommand(_GeneralExpenseCommand);

            //CarSaleIncomeCommand = new RelayCommand(_CarSaleIncomeCommand);
            //ProductSaleIncomeCommand = new RelayCommand(_ProductSaleIncomeCommand);

            NavigationGroups.Add(new NavigationGroup("Расходы")
                .Add(_CarExpenseCommand, "Расходы на авто")
                .Add(_GeneralExpenseCommand, "Общие расходы"));

            NavigationGroups.Add(new NavigationGroup("Доходы")
                .Add(_ProductSaleIncomeCommand, "Доходы от продажи товаров"));

            NavigationGroups.Add(new NavigationGroup("Контрагенты")
                .Add(_CounteragentCommand, "Контрагенты")
                .Add(_InteractionWithCounteragentCommand, "Взаимодействие с контрагентами"));

        }

        #endregion Constructor



        #region Commands

        //public RelayCommand TestCommand { get; }

        //public RelayCommand CounteragentCommand { get; }
        //public RelayCommand InteractionWithCounteragentCommand { get; }

        //public RelayCommand CarExpenseCommand { get; }
        //public RelayCommand GeneralExpenseCommand { get; }

        //public RelayCommand CarSaleIncomeCommand { get; }
        //public RelayCommand ProductSaleIncomeCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _TestCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _CounteragentCommand()
        {
            _tabService.OpenTab<CounteragentListViewModel>("Контрагенты");
        }

        private void _InteractionWithCounteragentCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _CarExpenseCommand()
        {
            _tabService.OpenTab<CarExpenseListViewModel>("Расходы на авто");
        }

        private void _GeneralExpenseCommand()
        {
            _tabService.OpenTab<GeneralExpenseListViewModel>("Общие расходы");
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