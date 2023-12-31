﻿using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.MainMenu.SalaryAndStaff.Salary;
using SADA.ViewModel.MainMenu.SalaryAndStaff.Staff;
using SADA.ViewModel.Start;
using System;

namespace SADA.ViewModel.Dialogs.MainMenu
{
    public class SalaryAndStaffDialogViewModel : DialogBase
    {
        #region Constructor

        public SalaryAndStaffDialogViewModel(ITabService tabService = null) : base(tabService)
        {
            //TestCommand = new RelayCommand(_TestCommand);
            //AccrualOfSalariesCommand = new RelayCommand(_AccrualOfSalariesCommand);
            //SalaryReportCommand = new RelayCommand(_SalaryReportCommand);
            //StaffCommand = new RelayCommand(_StaffCommand);

            NavigationGroups.Add(new NavigationGroup("Кадры")
                .Add(_StaffCommand, "Сотрудники"));

            NavigationGroups.Add(new NavigationGroup("Зарплата")
                .Add(_AccrualOfSalariesCommand, "Начисление зарплат")
                .Add(_SalaryReportCommand, "Отчет по зарплатам"));
        }

        #endregion Constructor

        #region Commands

        //public RelayCommand TestCommand { get; }
        //public RelayCommand AccrualOfSalariesCommand { get; }
        //public RelayCommand SalaryReportCommand { get; }
        //public RelayCommand StaffCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _TestCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _AccrualOfSalariesCommand()
        {
            _tabService.OpenTab<AccrualOfSalariesListViewModel>("Начисление зарплат");
        }

        private void _SalaryReportCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _StaffCommand()
        {
            _tabService.OpenTab<StaffListViewModel>("Сотрудники");
        }

        #endregion Commands implementations
    }
}