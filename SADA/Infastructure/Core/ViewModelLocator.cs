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

                    return App.Current.Services.GetService(typeof(MainViewModel)) as MainViewModel;
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
                return new TestViewModel();
            }
        }

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(dummy);
        }
    }
}
