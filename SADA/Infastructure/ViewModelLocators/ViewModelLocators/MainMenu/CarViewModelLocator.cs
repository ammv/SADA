using SADA.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.ViewModelLocators.MainMenu
{
    class CarViewModelLocator: ViewModelLocatorBase
    {
        public Car.CarViewModelLocator CarVml { get; } = new Car.CarViewModelLocator();
        public Car.OtherViewModelLocator OtherVml { get; } = new Car.OtherViewModelLocator();
        public Car.SalonViewModelLocator SalonVml { get; } = new Car.SalonViewModelLocator();
    }
}
