using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Car;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Car
{
    public class CarViewModelLocator : ViewModelLocatorBase
    {
        public PayToCounteragentViewModel PayToCounteragent
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockPayToCounteragentViewModel();
                }
                else
                {
                    return App.Current.GetService<PayToCounteragentViewModel>();
                }
            }
        }

        public PayToCounteragentListViewModel PayToCounteragentList
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockPayToCounteragentListViewModel();
                }
                else
                {
                    return App.Current.GetService<PayToCounteragentListViewModel>();
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