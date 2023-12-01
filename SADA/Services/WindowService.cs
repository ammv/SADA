using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.Services
{
    public class WindowService : IWindowService
    {
        private Window _lastOpenedWindow = null;
        public Window LastOpenedWindow => _lastOpenedWindow;

        public void ShowAndCloseWindow<TWindow>(Window currentWindow) where TWindow : Window, new()
        {
            _lastOpenedWindow = new TWindow();
            _lastOpenedWindow.Show();
            currentWindow.Close();
        }

        public void ShowDialogWindow<TWindow>() where TWindow : Window, new()
        {
            _lastOpenedWindow = new TWindow();
            _lastOpenedWindow.ShowDialog();
        }

        public void ShowWindow<TWindow>() where TWindow : Window, new()
        {
            _lastOpenedWindow = new TWindow();
            _lastOpenedWindow.Show();
        }
    }
}
