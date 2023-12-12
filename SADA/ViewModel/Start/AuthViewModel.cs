using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer;
using FadeWpf;
using SADA.Services;
using SADA.View.Start;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.Start
{
    public class AuthViewModel : ObservableObject
    {
        #region Constructor

        public AuthViewModel(WindowFadeChanger windowFadeChanger, IUserService userService, IWindowService windowService)
        {
            AuthCommand = new AsyncRelayCommand(_AuthCommand, _AuthCommandCanExecute);
            this._windowFadeChanger = windowFadeChanger;
            this._userService = userService;
            this._windowService = windowService;

            PropertyChanged += (s, e) => AuthCommand.NotifyCanExecuteChanged();
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
        private bool _isLoading = false;

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

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion Properties

        #region Commands

        public AsyncRelayCommand AuthCommand { get; }

        #endregion Commands

        #region Command implementations8

        private async Task _AuthCommand()
        {
            //Window wnd = App.Current.MainWindow;
            //wnd.Hide();
            //(App.Current.Services.GetService(typeof(MainView)) as MainView).Show();
            //wnd.Close();

            IsLoading = true;

            User user = null;

            await Task.Run(() =>
            {
                user = _userService.GetUser(_login);
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
                if (user.Login != _login)
                {
                    HandyControl.Controls.MessageBox.Show("Неверный логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                
            });


            IsLoading = false;

            if (user != null)
            {
                if (App.Current.CurrentUser?.ID != user.ID && App.Current.UserTabs != null)
                {
                    App.Current.UserTabs.Clear();
                    App.Current.UserTabs = null;
                }
                App.Current.CurrentUser = user;

                _windowService.ShowAndCloseWindow<MainView>(_windowService.LastOpenedWindow);
            }
        }

        private bool _AuthCommandCanExecute()
        {
            return (!string.IsNullOrEmpty(_login) && !string.IsNullOrEmpty(_password)) || _isLoading;
        }

        #endregion Command implementations8
    }
}