using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Product.Income;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu.Product
{
    class IncomeViewModelLocator: ViewModelLocatorBase
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
