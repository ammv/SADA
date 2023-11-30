using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Themes;
using HandyControl.Tools;
using SADA.Services;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace SADA.ViewModel.Utils
{
    public class WindowTopButtonsViewModel : ObservableObject
    {
        #region Fields

        private bool _isCloseEnabled = true;
        private bool _isRecoverOrUnwrapEnabled = true;
        private bool _isWrapEnabled = true;

        private bool _isThemeChangerEnabled = true;

        private bool _isCalculatorEnabled = true;
        private bool _isCalendarEnabled = true;

        private bool _isThemeChangerSeparatorEnabled = true;
        private bool _isOtherSeparatorEnabled = true;


        private WindowState _windowState = WindowState.Maximized;
        private readonly IDialogService _dialogService;
        private readonly IWindowService _windowService;
        private bool _isMaximized = true;

        #endregion Fields

        #region Properties
        public bool IsMaximized
        {
            get => _isMaximized;
            set => SetProperty(ref _isMaximized, value);
        }
        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                if(SetProperty(ref _windowState, value))
                {
                    IsMaximized = value == WindowState.Maximized;
                }
            }

        }

        #region Window buttons

        public bool IsCloseEnabled
        {
            get => _isCloseEnabled;
            set => SetProperty(ref _isCloseEnabled, value);
        }

        public bool IsRecoverOrUnwrapEnabled
        {
            get => _isRecoverOrUnwrapEnabled;
            set => SetProperty(ref _isRecoverOrUnwrapEnabled, value);
        }

        public bool IsWrapEnabled
        {
            get => _isWrapEnabled;
            set => SetProperty(ref _isWrapEnabled, value);
        }

        #endregion Window buttons

        #region Theme buttons

        public bool IsThemeChangerEnabled
        {
            get => _isThemeChangerEnabled;
            set
            {
                if (SetProperty(ref _isThemeChangerEnabled, value))
                {
                    IsThemeChangerSeparatorEnabled = value;
                }
            }
        }

        public bool IsThemeChangerSeparatorEnabled
        {
            get => _isThemeChangerSeparatorEnabled;
            set => SetProperty(ref _isThemeChangerSeparatorEnabled, value);
        }

        #endregion 

        #region Other button

        public bool IsCalculatorEnabled
        {
            get => _isCalculatorEnabled;
            set
            {
                if (SetProperty(ref _isCalculatorEnabled, value))
                {
                    IsOtherSeparatorEnabled = value || _isCalendarEnabled;
                }
            }
        }

        public bool IsCalendarEnabled
        {
            get => _isCalendarEnabled;
            set
            {
                if (SetProperty(ref _isCalendarEnabled, value))
                {
                    IsOtherSeparatorEnabled = value || _isCalculatorEnabled;
                }
            }
        }

        public bool IsOtherSeparatorEnabled
        {
            get => _isOtherSeparatorEnabled;
            set => SetProperty(ref _isOtherSeparatorEnabled, value);
        }

        #endregion 


        #endregion Properties

        #region Constructor

        public WindowTopButtonsViewModel()
        {
            CloseWindowCommand = new RelayCommand<Window>(_closeWindowCommand);
            WrapWindowCommand = new RelayCommand<Window>(_wrapWindowCommand);
            RecoverOrUnwrapWindowCommand = new RelayCommand<Window>(_recoverOrUnwrapWindowCommand);
            ChangeThemeCommand = new RelayCommand(_ChangeThemeCommand);
            OpenCalculatorToolCommand = new RelayCommand(_OpenCalculatorToolCommand);
            OpenCalendarToolCommand = new RelayCommand(_OpenCalendarToolCommand);

            _dialogService = App.Current.GetService<IDialogService>();
            _windowService = App.Current.GetService<IWindowService>();
        }

        #endregion Constructor

        #region Commands

        public RelayCommand<Window> CloseWindowCommand { get; }
        public RelayCommand<Window> RecoverOrUnwrapWindowCommand { get; }
        public RelayCommand<Window> WrapWindowCommand { get; }

        public RelayCommand ChangeThemeCommand { get; }
        public RelayCommand OpenCalculatorToolCommand { get; }
        public RelayCommand OpenCalendarToolCommand { get; }

        #endregion Commands

        #region Commands implementations
            
        private void _ChangeThemeCommand()
        {
            ThemeManager.Current.ApplicationTheme = ThemeManager.Current.ApplicationTheme == ApplicationTheme.Light ? ApplicationTheme.Dark : ApplicationTheme.Light;
            ThemeAnimationHelper.AnimateTheme(_windowService.LastOpenedWindow, ThemeAnimationHelper.SlideDirection.Top, 0.4, 0.7, 1);
        }

        private void _OpenCalculatorToolCommand()
        {
            Process.Start("calc.exe");
        }

        private void _OpenCalendarToolCommand()
        {
            Process.Start("calc.exe");
        }

        private void _closeWindowCommand(Window window)
        {
            if (window != null)
            {
                var result = _dialogService.ShowMessageBox("Вопрос", "Вы уверены что хотите выйти?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    window.Close();
                }
            }
        }

        private void _recoverOrUnwrapWindowCommand(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                if (window.MinHeight != 0.0 && window.MinWidth != 0.0)
                {
                    window.Height = window.MinHeight;
                    window.Width = window.MinWidth;
                }

                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
            }
        }

        private void _wrapWindowCommand(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }

        #endregion Commands implementations
    }
}