using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

namespace SADA.ViewModel.Utils
{
    public class WindowTopButtonsViewModel : ObservableObject
    {
        #region Fields

        WindowState _windowState = WindowState.Maximized;
        bool _isCloseEnabled = true;
        bool _isRecoverOrUnwrapEnabled = true;
        bool _isWrapEnabled = true;

        #endregion

        #region Properties

        public WindowState WindowState
        {
            get => _windowState;
            set => SetProperty(ref _windowState, value);
        }

        public bool IsMaximized
        {
            get => _windowState == WindowState.Maximized;
        }

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

        #endregion

        public WindowTopButtonsViewModel()
        {
            CloseWindowCommand = new RelayCommand<Window>(_closeWindowCommand);
            WrapWindowCommand = new RelayCommand<Window>(_wrapWindowCommand);
            RecoverOrUnwrapWindowCommand = new RelayCommand<Window>(_recoverOrUnwrapWindowCommand);
        }

        #region Commands

        public ICommand CloseWindowCommand { get; }

        public ICommand RecoverOrUnwrapWindowCommand { get; }

        public ICommand WrapWindowCommand { get; }


        #endregion

        #region Commands implementations

        private void _closeWindowCommand(Window window)
        {
            if (window != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Вопрос", MessageBoxButton.YesNo);
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

        #endregion
    }
}