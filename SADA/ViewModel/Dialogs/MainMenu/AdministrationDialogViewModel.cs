using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using SADA.ViewModel.Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SADA.Infastructure.Dialogs.ViewModel.MainMenu
{
    class AdministrationDialogViewModel : DialogBase
    {
        #region Fields
        #endregion

        #region Constructor

        public AdministrationDialogViewModel() : base()
        {
            TestCommand = new RelayCommand(_TestCommand);
            EventJournalCommand = new RelayCommand(_EventJournalCommand);
            UsersCommand = new RelayCommand(_UsersCommand);
            UserRoleManagementCommand = new RelayCommand(_UserRoleManagementCommand);
        }

        #endregion

        #region Properties
        #endregion

        #region Commands

        public RelayCommand TestCommand { get; }
        public RelayCommand EventJournalCommand { get; }
        public RelayCommand UsersCommand { get; }
        public RelayCommand UserRoleManagementCommand { get; }

        #endregion

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


        #endregion

        #region Other
        #endregion
    }
}
