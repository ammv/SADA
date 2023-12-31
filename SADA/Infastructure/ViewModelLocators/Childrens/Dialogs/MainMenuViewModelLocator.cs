﻿using SADA.Infastructure.Core;
using SADA.ViewModel.Dialogs.MainMenu;

namespace SADA.Infastructure.ViewModelLocators.Childrens.Dialogs
{
    public class MainMenuViewModelLocator : ViewModelLocatorBase
    {
        public AdministrationDialogViewModel AdministrationDialog
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new AdministrationDialogViewModel(null);
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
                    return new CarDialogViewModel(null);
                }
                else
                {
                    return App.Current.GetService<CarDialogViewModel>();
                }
            }
        }

        public HomeDialogViewModel HomeDialog
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

        public SalaryAndStaffDialogViewModel SalaryAndStaffDialog
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new SalaryAndStaffDialogViewModel();
                }
                else
                {
                    return App.Current.GetService<SalaryAndStaffDialogViewModel>();
                }
            }
        }
    }
}