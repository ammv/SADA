using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Home.Income;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Home
{
    public class IncomeViewModelLocator : ViewModelLocatorBase
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