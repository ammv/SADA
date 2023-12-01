using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Product.Sales;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Product
{
    public class SalesViewModelLocator : ViewModelLocatorBase
    {
        public ProductSaleViewModel ProductSale
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new ProductSaleViewModel();
                }
                else
                {
                    return App.Current.GetService<ProductSaleViewModel>();
                }
            }
        }
    }
}