using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.Childrens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators
{
    class MainViewModelLocator : ViewModelLocatorBase
    {
        public StartViewModelLocator StartVml { get; } = new StartViewModelLocator();
        public MainMenuViewModelLocator MainMenuVml { get; } = new MainMenuViewModelLocator();
        public DialogsViewModelLocator DialogsVml { get; } = new DialogsViewModelLocator();
        public UtilsViewModelLocator UtilsVml { get; } = new UtilsViewModelLocator();
    }
}
