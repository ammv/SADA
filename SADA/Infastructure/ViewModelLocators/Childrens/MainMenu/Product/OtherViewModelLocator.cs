using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Product.Other;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Product
{
    public class OtherViewModelLocator : ViewModelLocatorBase
    {
        public PriceListViewModel PriceList
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new PriceListViewModel();
                }
                else
                {
                    return App.Current.GetService<PriceListViewModel>();
                }
            }
        }

        public ProductRemainsViewModel ProductRemains
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new ProductRemainsViewModel();
                }
                else
                {
                    return App.Current.GetService<ProductRemainsViewModel>();
                }
            }
        }
    }
}