using SADA.Infastructure.Core;
using SADA.ViewModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens
{
    class UtilsViewModelLocator: ViewModelLocatorBase
    {

        public WindowTopButtonsViewModel WindowTopButtons
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new WindowTopButtonsViewModel();
                }
                else
                {
                    return App.Current.GetService<WindowTopButtonsViewModel>();
                }
            }
        }

    }
}
