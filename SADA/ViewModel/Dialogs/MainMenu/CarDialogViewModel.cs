using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.MainMenu.Car.Salon;
using SADA.ViewModel.Start;
using System;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    internal class CarDialogViewModel : DialogBase
    {
        private readonly ITabService _tabService;
        #region Constructor

        public CarDialogViewModel(ITabService tabService = null) : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            PayToCounteragentCommand = new RelayCommand(_PayToCounteragentCommand);
            PurchaseFromCounteragentCommand = new RelayCommand(_PurchaseFromCounteragentCommand);
            CarInSalonCommand = new RelayCommand(_CarInSalonCommand);
            EquipmentCommand = new RelayCommand(_EquipmentCommand);
            _tabService = tabService;
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
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _PurchaseFromCounteragentCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _CarInSalonCommand()
        {
            

            _tabService.OpenTab<CarInSalonViewModel>("Автомобили в салоне");
        }

        private void _EquipmentCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        #endregion Commands implementations
    }
}