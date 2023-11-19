using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Home.Expense;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Home
{
    internal class ExpenseViewModelLocator : ViewModelLocatorBase
    {
        public CarExpenseViewModel CarExpense
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new CarExpenseViewModel();
                }
                else
                {
                    return App.Current.GetService<CarExpenseViewModel>();
                }
            }
        }

        public GeneralExpenseViewModel GeneralExpense
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new GeneralExpenseViewModel();
                }
                else
                {
                    return App.Current.GetService<GeneralExpenseViewModel>();
                }
            }
        }
    }
}