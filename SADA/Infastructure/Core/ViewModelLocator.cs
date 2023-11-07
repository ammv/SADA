using SADA.Infastructure.Dialogs.ViewModel;
using SADA.ViewModel.Start;
using SADA.ViewModel.Utils;
using System.ComponentModel;
using System.Windows;

namespace SADA.Infastructure.Core
{
    class ViewModelLocator
    {
        private DependencyObject dummy = new DependencyObject();
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

                    return App.Current.Services.GetService(typeof(AuthViewModel)) as AuthViewModel;
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

        public MenuDialogViewModel MenuDialog
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MenuDialogViewModel();
                }
                return App.Current.GetService<MenuDialogViewModel>();
            }
        }

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(dummy);
        }
    }
}
