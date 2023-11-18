using SADA.Infastructure.Core;
using SADA.ViewModel.Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators
{
    class StartViewModelLocator: ViewModelLocatorBase
    {
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

        public WelcomeTabViewModel WelcomeTab
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new WelcomeTabViewModel();
                }
                return App.Current.GetService<WelcomeTabViewModel>();
            }
        }
    }
}
