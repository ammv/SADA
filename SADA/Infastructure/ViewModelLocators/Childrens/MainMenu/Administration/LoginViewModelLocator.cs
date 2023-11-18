using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Administration.Login;
using SADA.ViewModel.MainMenu.Administration.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Administration
{
    class LoginViewModelLocator: ViewModelLocatorBase
    {


        public EventJournalViewModel EventJournal
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new EventJournalViewModel();
                }
                else
                {
                    return App.Current.GetService<EventJournalViewModel>();
                }
            }
        }



    }
}
