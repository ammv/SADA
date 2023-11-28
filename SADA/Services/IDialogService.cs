using System.IO;
using System.Windows;

namespace SADA.Services
{
    public interface IDialogService
    {
        string FileDialogImageFilter { get; }
        MessageBoxResult ShowMessageBox(string caption, string message, MessageBoxButton messageBoxButton);
        FileInfo[] ShowFileDialog(string filter = null);
    }
}