using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu.Car
{
    class CarViewModelLocator: ViewModelLocatorBase
    {

        public PayToCounteragentViewModel PayToCounteragent
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new PayToCounteragentViewModel();
                }
                else
                {
                    return App.Current.GetService<PayToCounteragentViewModel>();
                }
            }
        }


        public PurchaseFromCounteragentViewModel PurchaseFromCounteragent
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new PurchaseFromCounteragentViewModel();
                }
                else
                {
                    return App.Current.GetService<PurchaseFromCounteragentViewModel>();
                }
            }
        }


    }
}
