using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.MainMenu.Car.Car;
using SADA.ViewModel.MainMenu.Car.Other;
using SADA.ViewModel.MainMenu.Car.Salon;
using SADA.ViewModel.Start;
using System;

namespace SADA.ViewModel.Dialogs.MainMenu
{
    public class CarDialogViewModel : DialogBase
    {
        #region Constructor

        public CarDialogViewModel(ITabService tabService = null) : base(tabService)
        {
            //TestCommand = new RelayCommand(_TestCommand);
            //PayToCounteragentCommand = new RelayCommand(_PayToCounteragentCommand);
            //PurchaseFromCounteragentCommand = new RelayCommand(_PurchaseFromCounteragentCommand);
            //CarInSalonCommand = new RelayCommand(_CarInSalonCommand);
            //EquipmentCommand = new RelayCommand(_EquipmentCommand);

            NavigationGroups.Add(new NavigationGroup("Автомобили")
                .Add(_PayToCounteragentCommand, "Оплата контрагентам за автомобиль")
                .Add(_PurchaseFromCounteragentCommand, "Плата от контрагентов за автомобиль"));

            NavigationGroups.Add(new NavigationGroup("Салон")
                .Add(_CarInSalonCommand, "Автомобили в салоне"));

            NavigationGroups.Add(new NavigationGroup("Прочее")
                .Add(_EquipmentCommand, "Оплата контрагентам за автомобиль"));
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
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _PayToCounteragentCommand()
        {
            _tabService.OpenTab<PayToCounteragentListViewModel>("Оплата контрагентам за автомобили");
        }

        private void _PurchaseFromCounteragentCommand()
        {
            _tabService.OpenTab<PurchaseFromCounteragentListViewModel>("Оплата от контрагентов за автомобили");
        }

        private void _CarInSalonCommand()
        {
            _tabService.OpenTab<CarInSalonListViewModel>("Автомобили в салоне");
        }

        private void _EquipmentCommand()
        {
            _tabService.OpenTab<EquipmentListViewModel>("Комплектации автомобилей");
        }

        #endregion Commands implementations
    }
}