using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.MainMenu.Home.Expense;
using SADA.ViewModel.Start;
using System;
using System.Collections.Generic;

namespace SADA.ViewModel.Dialogs.MainMenu
{
    public class ManualDialogViewModel : DialogBase
    {
        #region Constructor

        public ManualDialogViewModel(ITabService tabService = null) : base(tabService)
        {
            TestCommand = new RelayCommand(_TestCommand);

            NavigationGroup carGroup = new NavigationGroup("Автомобиль");

            carGroup.Add(_TestCommand, "Марки автомобилей");
            carGroup.Add(_TestCommand, "Модели автомобилей");
            carGroup.Add(_TestCommand, "Марки топлива");
            carGroup.Add(_TestCommand, "Виды двигателей");
            carGroup.Add(_TestCommand, "Виды кузовов");
            carGroup.Add(_TestCommand, "Виды коробки передач");

            NavigationGroup expenseGroup = new NavigationGroup("Расходы");

            expenseGroup.Add(_TestCommand, "Группы расходов");
            expenseGroup.Add(_TestCommand, "Типы расходов");

            NavigationGroups.Add(carGroup);
            NavigationGroups.Add(expenseGroup);
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