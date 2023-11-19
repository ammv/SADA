using SADA.ViewModel.Start;
using SADA.ViewModel.Utils;
using System.ComponentModel;
using System.Windows;

namespace SADA.Infastructure.Core
{
    internal class ViewModelLocator
    {
        private DependencyObject dummy = new DependencyObject();
        private _Dialogs _dialogs = new _Dialogs();
        public _Dialogs Dialogs { get => _dialogs; }

        public AuthViewModel Auth
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockAuthViewModel();
                }
                else
                {
                    return App.Current.GetService<AuthViewModel>();
                }
            }
        }

        public MainViewModel Main
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockMainViewModel();
                }
                else
                {
                    return App.Current.GetService<MainViewModel>();
                }
            }
        }

        public WindowTopButtonsViewModel UtilsWindowTopButtons
        {
            get
            {
                return new WindowTopButtonsViewModel();
            }
        }

        public TestViewModel Test
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new TestViewModel();
                }
                return App.Current.GetService<TestViewModel>();
            }
        }

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(dummy);
        }

        internal class _Dialogs
        {
            private DependencyObject dummy = new DependencyObject();

            private bool IsInDesignMode()
            {
                return DesignerProperties.GetIsInDesignMode(dummy);
            }

            public Dialogs.ViewModel.MainMenu.AdministrationDialogViewModel AdministrationDialog
            {
                get
                {
                    if (IsInDesignMode())
                    {
                        return new Dialogs.ViewModel.MainMenu.AdministrationDialogViewModel();
                    }
                    return App.Current.GetService<Dialogs.ViewModel.MainMenu.AdministrationDialogViewModel>();
                }
            }

            public Dialogs.ViewModel.MainMenu.CarDialogViewModel CarDialog
            {
                get
                {
                    if (IsInDesignMode())
                    {
                        return new Dialogs.ViewModel.MainMenu.CarDialogViewModel();
                    }
                    return App.Current.GetService<Dialogs.ViewModel.MainMenu.CarDialogViewModel>();
                }
            }

            public Dialogs.ViewModel.MainMenu.HomeDialogViewModel HomeDialog
            {
                get
                {
                    if (IsInDesignMode())
                    {
                        return new Dialogs.ViewModel.MainMenu.HomeDialogViewModel();
                    }
                    return App.Current.GetService<Dialogs.ViewModel.MainMenu.HomeDialogViewModel>();
                }
            }

            public Dialogs.ViewModel.MainMenu.ManualDialogViewModel ManualDialog
            {
                get
                {
                    if (IsInDesignMode())
                    {
                        return new Dialogs.ViewModel.MainMenu.ManualDialogViewModel();
                    }
                    return App.Current.GetService<Dialogs.ViewModel.MainMenu.ManualDialogViewModel>();
                }
            }

            public Dialogs.ViewModel.MainMenu.ProductDialogViewModel ProductDialog
            {
                get
                {
                    if (IsInDesignMode())
                    {
                        return new Dialogs.ViewModel.MainMenu.ProductDialogViewModel();
                    }
                    return App.Current.GetService<Dialogs.ViewModel.MainMenu.ProductDialogViewModel>();
                }
            }

            public Dialogs.ViewModel.MainMenu.SalaryAndStaffDialogViewModel SalaryAndStaff
            {
                get
                {
                    if (IsInDesignMode())
                    {
                        return new Dialogs.ViewModel.MainMenu.SalaryAndStaffDialogViewModel();
                    }
                    return App.Current.GetService<Dialogs.ViewModel.MainMenu.SalaryAndStaffDialogViewModel>();
                }
            }
        }
    }
}