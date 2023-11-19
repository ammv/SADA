using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Product;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu
{
    internal class ProductViewModelLocator : ViewModelLocatorBase
    {
        public IncomeViewModelLocator IncomeVml { get; } = new IncomeViewModelLocator();
        public OtherViewModelLocator OtherVml { get; } = new OtherViewModelLocator();
        public SalesViewModelLocator SalesVml { get; } = new SalesViewModelLocator();
    }
}