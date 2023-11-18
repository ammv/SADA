using SADA.Infastructure.Core;
using SADA.Infastructure.Dialogs.ViewModel.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.Dialogs
{
    class MainMenuViewModelLocator: ViewModelLocatorBase
    {

        public AdministrationDialogViewModel AdministrationDialog
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new AdministrationDialogViewModel();
                }
                else
                {
                    return App.Current.GetService<AdministrationDialogViewModel>();
                }
            }
        }


        public CarDialogViewModel CarDialog
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new CarDialogViewModel();
                }
                else
                {
                    return App.Current.GetService<CarDialogViewModel>();
                }
            }
        }


        public HomeDialogViewModel Home
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new HomeDialogViewModel();
                }
                else
                {
                    return App.Current.GetService<HomeDialogViewModel>();
                }
            }
        }


        public ManualDialogViewModel ManualDialog
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new ManualDialogViewModel();
                }
                else
                {
                    return App.Current.GetService<ManualDialogViewModel>();
                }
            }
        }


        public ProductDialogViewModel ProductDialog
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new ProductDialogViewModel();
                }
                else
                {
                    return App.Current.GetService<ProductDialogViewModel>();
                }
            }
        }


        public SalaryAndStaffViewModel SalaryAndStaff
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new SalaryAndStaffViewModel();
                }
                else
                {
                    return App.Current.GetService<SalaryAndStaffViewModel>();
                }
            }
        }


    }
}
