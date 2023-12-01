using HandyControl.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.Services
{
    class DialogService : IDialogService
    {
        private readonly MessageBoxInfo _messageBoxInfo;

        public DialogService()
        {
            _messageBoxInfo = new MessageBoxInfo
            {
                YesContent = "Да",
                NoContent = "Нет",
                CancelContent = "Отмена",
                ConfirmContent = "Ок"
            };
        }

        public string FileDialogImageFilter => "Images(*.png,*jpeg,*.jpg,*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files(*.*)|*.*";

        public FileInfo[] ShowFileDialog(string filter = null, bool multiChoice = false)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.Multiselect = multiChoice;
            if(dialog.ShowDialog() == true)
            {
                return dialog.FileNames.Select(f => new FileInfo(f)).ToArray();
            }
            return null;
        }

        public MessageBoxResult ShowMessageBox(string caption, string message, MessageBoxButton messageBoxButton)
        {
            _messageBoxInfo.Caption = caption;
            _messageBoxInfo.Message = message;
            _messageBoxInfo.Button = messageBoxButton;

            return HandyControl.Controls.MessageBox.Show(_messageBoxInfo);
        }
    }
}
