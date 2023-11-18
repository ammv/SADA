using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu
{
    class HomeViewModelLocator: ViewModelLocatorBase
    {
        public CounteragentViewModelLocator CounteragentVml { get; } = new CounteragentViewModelLocator();
        public ExpenseViewModelLocator ExpenseVml { get; } = new ExpenseViewModelLocator();
        public IncomeViewModelLocator IncomeVml { get; } = new IncomeViewModelLocator();
    }
}
