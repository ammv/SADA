using HandyControl.Data;
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
        public MessageBoxResult ShowMessageBox(string caption, string message, MessageBoxButton messageBoxButton)
        {
            _messageBoxInfo.Caption = caption;
            _messageBoxInfo.Message = message;
            _messageBoxInfo.Button = messageBoxButton;

            return HandyControl.Controls.MessageBox.Show(_messageBoxInfo);
        }
    }
}
