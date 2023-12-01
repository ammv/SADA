using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.Services;
using SADA.ViewModel.Start;
using System;

namespace SADA.ViewModel.Dialogs.MainMenu
{
    public class AdministrationDialogViewModel : DialogBase
    {
        private readonly ITabService _tabService;
        #region Constructor

        public AdministrationDialogViewModel(ITabService tabService = null) : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            EventJournalCommand = new RelayCommand(_EventJournalCommand);
            UsersCommand = new RelayCommand(_UsersCommand);
            UserRoleManagementCommand = new RelayCommand(_UserRoleManagementCommand);
            _tabService = tabService;
        }

        #endregion Constructor

        #region Commands

        public RelayCommand TestCommand { get; }
        public RelayCommand EventJournalCommand { get; }
        public RelayCommand UsersCommand { get; }
        public RelayCommand UserRoleManagementCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _TestCommand()
        {
            //var testVm = App.Current.GetService<TestViewModel>();
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _EventJournalCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _UsersCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        private void _UserRoleManagementCommand()
        {
            _tabService.OpenTab<TestViewModel>(nameof(TestViewModel));
        }

        #endregion Commands implementations
    }
}