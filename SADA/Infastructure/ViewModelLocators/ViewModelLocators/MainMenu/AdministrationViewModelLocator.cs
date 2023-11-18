﻿using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu
{
    class AdministrationViewModelLocator: ViewModelLocatorBase
    {
        public SystemViewModelLocator SystemVml { get; } = new SystemViewModelLocator();
        public LoginViewModelLocator LoginVml { get; } = new LoginViewModelLocator();
    }
}
