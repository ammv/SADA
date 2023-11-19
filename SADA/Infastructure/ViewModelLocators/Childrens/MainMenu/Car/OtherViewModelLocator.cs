using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Other;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Car
{
    internal class OtherViewModelLocator : ViewModelLocatorBase
    {
        public EquipmentViewModel Equipment
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new EquipmentViewModel();
                }
                else
                {
                    return App.Current.GetService<EquipmentViewModel>();
                }
            }
        }
    }
}