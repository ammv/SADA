using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Administration.System;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Administration
{
    internal class LoginViewModelLocator : ViewModelLocatorBase
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