using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Home;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu
{
    internal class HomeViewModelLocator : ViewModelLocatorBase
    {
        public CounteragentViewModelLocator CounteragentVml { get; } = new CounteragentViewModelLocator();
        public ExpenseViewModelLocator ExpenseVml { get; } = new ExpenseViewModelLocator();
        public IncomeViewModelLocator IncomeVml { get; } = new IncomeViewModelLocator();
    }
}