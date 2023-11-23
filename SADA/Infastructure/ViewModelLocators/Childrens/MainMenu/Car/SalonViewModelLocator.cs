using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Salon;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Car
{
    internal class SalonViewModelLocator : ViewModelLocatorBase
    {
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

        public CarInSalonFormViewModel CarInSalonForm
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockCarInSalonFormViewModel();
                }
                else
                {
                    return App.Current.GetService<CarInSalonFormViewModel>();
                }
            }
        }
    }
}