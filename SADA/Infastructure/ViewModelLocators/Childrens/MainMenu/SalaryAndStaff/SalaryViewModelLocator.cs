using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.SalaryAndStaff.Salary;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.SalaryAndStaff
{
    public class SalaryViewModelLocator : ViewModelLocatorBase
    {
        public AccrualOfSalariesViewModel AccrualOfSalaries
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new AccrualOfSalariesViewModel(null, null);
                }
                else
                {
                    return App.Current.GetService<AccrualOfSalariesViewModel>();
                }
            }
        }

        public AccrualOfSalariesListViewModel AccrualOfSalariesList
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new AccrualOfSalariesListViewModel(null, null);
                }
                else
                {
                    return App.Current.GetService<AccrualOfSalariesListViewModel>();
                }
            }
        }

        public SalaryReportViewModel SalaryReport
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new SalaryReportViewModel();
                }
                else
                {
                    return App.Current.GetService<SalaryReportViewModel>();
                }
            }
        }
    }
}