using SADA.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators
{
    class DialogsViewModelLocator: ViewModelLocatorBase
    {
        public Dialogs.MainMenuViewModelLocator MainMenuVml { get; } = new Dialogs.MainMenuViewModelLocator();
    }
}
