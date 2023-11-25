using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Salon;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Car
{
    internal class SalonViewModelLocator : ViewModelLocatorBase
    {
        public CarInSalonListViewModel CarInSalonList
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

        public CarInSalonViewModel CarInSalon
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