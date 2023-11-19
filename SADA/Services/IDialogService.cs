using System.Windows;

namespace SADA.Services
{
    public interface IDialogService
    {
        MessageBoxResult ShowMessageBox(string caption, string message, MessageBoxButton messageBoxButton);
    }
}