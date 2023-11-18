using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.SalaryAndStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu
{
    class SalaryAndStaffViewModelLocator: ViewModelLocatorBase
    {
        public SalaryViewModelLocator SalaryVml { get; } = new SalaryViewModelLocator();
        public StaffViewModelLocator StaffVml { get; } = new StaffViewModelLocator();
    }
}
