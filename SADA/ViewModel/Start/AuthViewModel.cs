using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer;
using FadeWpf;
using SADA.Services;
using SADA.View.Start;
using System.Windows;

namespace SADA.ViewModel.Start
{
    public class AuthViewModel : ObservableObject
    {
        #region Constructor

        public AuthViewModel(WindowFadeChanger windowFadeChanger, IUserService userService, IWindowService windowService)
        {
            AuthCommand = new RelayCommand(_AuthCommand, _AuthCommandCanExecute);
            this._windowFadeChanger = windowFadeChanger;
            this._userService = userService;
            this._windowService = windowService;
        }

        // For mock
        protected AuthViewModel()
        {

        }

        #endregion Constructor

        #region Fields

        private string _password = "admin";
        private string _login = "admin";
        private readonly WindowFadeChanger _windowFadeChanger;
        private readonly IUserService _userService;
        private readonly IWindowService _windowService;

        #endregion Fields

        #region Properties

        public string Login
        {
            get => _login;
            set
            {
                if (SetProperty(ref _login, value))
                {
                    AuthCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    AuthCommand.NotifyCanExecuteChanged();
                }
            }
        }

        #endregion Properties

        #region Commands

        public RelayCommand AuthCommand { get; }

        #endregion Commands

        #region Command implementations8

        private void _AuthCommand()
        {
            //Window wnd = App.Current.MainWindow;
            //wnd.Hide();
            //(App.Current.Services.GetService(typeof(MainView)) as MainView).Show();
            //wnd.Close();

            User user = _userService.GetUser(_login);
            if (user == null)
            {
                //MessageBox.Show("Пользователь с таким логином не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                HandyControl.Controls.MessageBox.Show("Пользователь с таким логином не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!_userService.CheckPassword(user, _password))
            {
                HandyControl.Controls.MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            if(App.Current.CurrentUser?.ID != user.ID && App.Current.UserTabs != null)
            {
                App.Current.UserTabs.Clear();
                App.Current.UserTabs = null;
            }
            App.Current.CurrentUser = user;

            //_windowFadeChanger.Change(App.Current.MainWindow,
            //    App.Current.Services.GetService(typeof(MainView)) as MainView);
            _windowService.ShowAndCloseWindow<MainView>(_windowService.LastOpenedWindow);
        }

        private bool _AuthCommandCanExecute()
        {
            return !string.IsNullOrEmpty(_login) && !string.IsNullOrEmpty(_password);
        }

        #endregion Command implementations8
    }
}