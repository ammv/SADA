using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.ViewModel.Start;
using System;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    internal class AdministrationDialogViewModel : DialogBase
    {
        #region Constructor

        public AdministrationDialogViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            EventJournalCommand = new RelayCommand(_EventJournalCommand);
            UsersCommand = new RelayCommand(_UsersCommand);
            UserRoleManagementCommand = new RelayCommand(_UserRoleManagementCommand);
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
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = DateTime.Now.ToLongDateString();

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _EventJournalCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "Журнал событий";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _UsersCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "Пользователи";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        private void _UserRoleManagementCommand()
        {
            var testVm = App.Current.GetService<TestViewModel>();

            testVm.Name = "Управление ролями пользователей";

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(testVm));
        }

        #endregion Commands implementations
    }
}