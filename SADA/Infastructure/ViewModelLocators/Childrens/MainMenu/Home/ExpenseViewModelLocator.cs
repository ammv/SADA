using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Home.Expense;
using SADA.ViewModel.MainMenu.Home.Expense.Mocks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Home
{
    public class ExpenseViewModelLocator : ViewModelLocatorBase
    {
        public CarExpenseViewModel CarExpense
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockCarExpenseViewModel();
                }
                else
                {
                    return App.Current.GetService<CarExpenseViewModel>();
                }
            }
        }

        public CarExpenseListViewModel CarExpenseList
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockCarExpenseListViewModel();
                }
                else
                {
                    return App.Current.GetService<CarExpenseListViewModel>();
                }
            }
        }

        public GeneralExpenseViewModel GeneralExpense
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockGeneralExpenseViewModel();
                }
                else
                {
                    return App.Current.GetService<GeneralExpenseViewModel>();
                }
            }
        }

        public GeneralExpenseListViewModel GeneralExpenseList
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockGeneralExpenseListViewModel();
                }
                else
                {
                    return App.Current.GetService<GeneralExpenseListViewModel>();
                }
            }
        }
    }
}