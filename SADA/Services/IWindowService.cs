using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.Services
{
    public interface IWindowService
    {
        Window LastOpenedWindow { get; }

        void ShowWindow<TWindow>() where TWindow : Window, new();
        void ShowAndCloseWindow<TWindow>(Window currentWindow) where TWindow : Window, new();
        void ShowDialogWindow<TWindow>() where TWindow : Window, new();
    }
}
