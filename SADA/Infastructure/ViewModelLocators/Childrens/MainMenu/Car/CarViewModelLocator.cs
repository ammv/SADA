using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Car;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Car
{
    internal class CarViewModelLocator : ViewModelLocatorBase
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