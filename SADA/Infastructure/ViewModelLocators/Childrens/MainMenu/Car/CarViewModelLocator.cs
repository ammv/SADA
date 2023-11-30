using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Car;
using SADA.ViewModel.MainMenu.Car.Car.Mocks;

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
                    return new MockPurchaseFromCounteragentViewModel();
                }
                else
                {
                    return App.Current.GetService<PurchaseFromCounteragentViewModel>();
                }
            }
        }

        public PurchaseFromCounteragentListViewModel PurchaseFromCounteragentList
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockPurchaseFromCounteragentListViewModel();
                }
                else
                {
                    return App.Current.GetService<PurchaseFromCounteragentListViewModel>();
                }
            }
        }


    }
}