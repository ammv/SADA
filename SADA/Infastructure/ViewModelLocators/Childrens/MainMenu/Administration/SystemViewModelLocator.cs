using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Administration.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Administration
{
    class SystemViewModelLocator: ViewModelLocatorBase
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
