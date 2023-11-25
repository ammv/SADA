using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Salon;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Car
{
    internal class SalonViewModelLocator : ViewModelLocatorBase
    {
        public CarInSalonListViewModel CarInSalon
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockCarInSalonListViewModel();
                }
                else
                {
                    return App.Current.GetService<CarInSalonListViewModel>();
                }
            }
        }

        public CarInSalonViewModel CarInSalonForm
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockCarInSalonViewModel();
                }
                else
                {
                    return App.Current.GetService<CarInSalonViewModel>();
                }
            }
        }
    }
}