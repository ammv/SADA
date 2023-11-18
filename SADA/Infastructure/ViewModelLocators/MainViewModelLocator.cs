using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.ViewModelLocators;
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
    }
}
