using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.SalaryAndStaff.Staff;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.SalaryAndStaff
{
    internal class StaffViewModelLocator : ViewModelLocatorBase
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