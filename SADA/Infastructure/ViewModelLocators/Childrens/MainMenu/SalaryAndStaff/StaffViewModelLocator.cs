using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.SalaryAndStaff.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.SalaryAndStaff
{
    class StaffViewModelLocator: ViewModelLocatorBase
    {

        public StaffViewModel Staff
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new StaffViewModel();
                }
                else
                {
                    return App.Current.GetService<StaffViewModel>();
                }
            }
        }

    }
}
