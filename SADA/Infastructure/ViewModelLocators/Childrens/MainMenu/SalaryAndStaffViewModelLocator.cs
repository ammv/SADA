using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.SalaryAndStaff;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu
{
    internal class SalaryAndStaffViewModelLocator : ViewModelLocatorBase
    {
        public SalaryViewModelLocator SalaryVml { get; } = new SalaryViewModelLocator();
        public StaffViewModelLocator StaffVml { get; } = new StaffViewModelLocator();
    }
}