using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Product.Income;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Product
{
    public class IncomeViewModelLocator : ViewModelLocatorBase
    {
        public ProductIncomeViewModel ProductIncome
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new ProductIncomeViewModel();
                }
                else
                {
                    return App.Current.GetService<ProductIncomeViewModel>();
                }
            }
        }
    }
}