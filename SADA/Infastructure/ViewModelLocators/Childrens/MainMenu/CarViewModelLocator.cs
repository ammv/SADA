using SADA.Infastructure.Core;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu
{
    public class CarViewModelLocator : ViewModelLocatorBase
    {
        public Car.CarViewModelLocator CarVml { get; } = new Car.CarViewModelLocator();
        public Car.OtherViewModelLocator OtherVml { get; } = new Car.OtherViewModelLocator();
        public Car.SalonViewModelLocator SalonVml { get; } = new Car.SalonViewModelLocator();
    }
}