using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.SalaryAndStaff.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu.SalaryAndStaff
{
    class SalaryViewModelLocator: ViewModelLocatorBase
    {

        public AccrualOfSalariesViewModel AccrualOfSalaries
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new AccrualOfSalariesViewModel();
                }
                else
                {
                    return App.Current.GetService<AccrualOfSalariesViewModel>();
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
