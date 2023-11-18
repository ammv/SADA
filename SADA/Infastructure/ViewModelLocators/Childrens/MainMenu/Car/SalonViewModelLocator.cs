using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Salon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Car
{
    class SalonViewModelLocator: ViewModelLocatorBase
    {

        public CarInSalonViewModel CarInSalon
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new CarInSalonViewModel();
                }
                else
                {
                    return App.Current.GetService<CarInSalonViewModel>();
                }
            }
        }

    }
}
