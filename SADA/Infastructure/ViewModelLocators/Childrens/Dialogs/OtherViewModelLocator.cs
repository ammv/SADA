using SADA.Infastructure.Core;
using SADA.ViewModel.Dialogs.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.Dialogs
{
    public class OtherViewModelLocator : ViewModelLocatorBase
    {
        public CounteragentContactPersonDialogViewModel CounteragentContactPersonDialog
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new CounteragentContactPersonDialogViewModel(null);
                }
                else
                {
                    return App.Current.GetService<CounteragentContactPersonDialogViewModel>();
                }
            }
        }
    }
}
