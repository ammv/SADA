using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Product.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu.Product
{
    class SalesViewModelLocator: ViewModelLocatorBase
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
