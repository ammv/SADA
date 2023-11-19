using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SADA.Services;
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
        private WindowState _windowState = WindowState.Maximized;
        private readonly IDialogService _dialogService;

        #endregion Fields

        #region Properties

        public bool IsCloseEnabled
        {
            get => _isCloseEnabled;
            set => SetProperty(ref _isCloseEnabled, value);
        }

        public bool IsMaximized
        {
            get => _windowState == WindowState.Maximized;
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

        public WindowState WindowState
        {
            get => _windowState;
            set => SetProperty(ref _windowState, value);
        }

        #endregion Properties

        #region Constructor

        public WindowTopButtonsViewModel(IDialogService dialogService)
        {
            CloseWindowCommand = new RelayCommand<Window>(_closeWindowCommand);
            WrapWindowCommand = new RelayCommand<Window>(_wrapWindowCommand);
            RecoverOrUnwrapWindowCommand = new RelayCommand<Window>(_recoverOrUnwrapWindowCommand);

            _dialogService = dialogService;
        }

        protected internal WindowTopButtonsViewModel() { }

        #endregion Constructor

        #region Commands

        public ICommand CloseWindowCommand { get; }

        public ICommand RecoverOrUnwrapWindowCommand { get; }

        public ICommand WrapWindowCommand { get; }

        #endregion Commands

        #region Commands implementations

        private void _closeWindowCommand(Window window)
        {
            if (window != null)
            {
                var result = _dialogService.ShowMessageBox("Вопрос", "Вы уверены что хотите выйти?",  MessageBoxButton.YesNo);
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