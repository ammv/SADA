using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Home.Income;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu.Home
{
    class IncomeViewModelLocator: ViewModelLocatorBase
    {

        public CarSaleIncomeViewModel CarSaleIncome
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new CarSaleIncomeViewModel();
                }
                else
                {
                    return App.Current.GetService<CarSaleIncomeViewModel>();
                }
            }
        }


        public ProductSaleIncomeViewModel ProductSaleIncome
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new ProductSaleIncomeViewModel();
                }
                else
                {
                    return App.Current.GetService<ProductSaleIncomeViewModel>();
                }
            }
        }


    }
}
