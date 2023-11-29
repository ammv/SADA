using SADA.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.Helpers
{
    public static class DbEntityValidationExceptionHelper
    {
        private static IDialogService _dialogService = App.Current.GetService<IDialogService>();
        public static void ShowException(DbEntityValidationException ex)
        {
            foreach (var entityValidationError in ex.EntityValidationErrors)
            {
                string entityValidationErrorMsg = string.Join("\n\n", entityValidationError.ValidationErrors.Select(x => x.ErrorMessage));
                _dialogService.ShowMessageBox("Ошибка EntityFramework", entityValidationErrorMsg, MessageBoxButton.OK);
            }
        }
    }
}
