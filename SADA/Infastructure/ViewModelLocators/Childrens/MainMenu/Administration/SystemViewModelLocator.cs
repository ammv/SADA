using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Administration.Login;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Administration
{
    internal class SystemViewModelLocator : ViewModelLocatorBase
    {
        public UserRoleManagementViewModel UserRoleManagement
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new UserRoleManagementViewModel();
                }
                else
                {
                    return App.Current.GetService<UserRoleManagementViewModel>();
                }
            }
        }

        public UsersViewModel Users
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new UsersViewModel();
                }
                else
                {
                    return App.Current.GetService<UsersViewModel>();
                }
            }
        }
    }
}