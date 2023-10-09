using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SADA.ViewModel.Start
{
    public class AuthorizationViewModel: ObservableObject
    {
        #region Fields

        private string _login;
        private string _password;

        #endregion

        #region Properties

        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        #endregion

        #region Commands

        public ICommand LoginCommand { get; }

        #endregion

        #region Command implementations

        private void _LoginCommand()
        {
            MessageBox.Show($"{Login} {Password}");
        }

        #endregion

        public AuthorizationViewModel()
        {
            LoginCommand = new RelayCommand(_LoginCommand);
        }

    }
}
