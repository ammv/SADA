using HandyControl.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        public string FileDialogImageFilter => "PNG Images(*.png)|*.png|JPG Images(*.img)|*.jpg|JPEG Images(*.jpg)|*.jpeg|*.bmp| All files(*.*)|*.*";

        public string[] ShowFileDialog(string filter = null)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.Multiselect = true; ;
            if(dialog.ShowDialog() == true)
            {
                return dialog.FileNames;
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
