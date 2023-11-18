﻿using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu
{
    class ProductViewModelLocator: ViewModelLocatorBase
    {
        public IncomeViewModelLocator IncomeVml { get; } = new IncomeViewModelLocator();
        public OtherViewModelLocator OtherVml { get; } = new OtherViewModelLocator();
        public SalesViewModelLocator SalesVml { get; } = new SalesViewModelLocator();
    }
}
