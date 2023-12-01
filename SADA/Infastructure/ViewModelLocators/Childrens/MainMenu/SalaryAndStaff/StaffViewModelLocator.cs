using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.SalaryAndStaff.Staff;
using SADA.ViewModel.MainMenu.SalaryAndStaff.Staff.Mocks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.SalaryAndStaff
{
    public class StaffViewModelLocator : ViewModelLocatorBase
    {
        public StaffViewModel Staff
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockStaffViewModel();
                }
                else
                {
                    return App.Current.GetService<StaffViewModel>();
                }
            }
        }

        public StaffListViewModel StaffList
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockStaffListViewModel();
                }
                else
                {
                    return App.Current.GetService<StaffListViewModel>();
                }
            }
        }
    }
}