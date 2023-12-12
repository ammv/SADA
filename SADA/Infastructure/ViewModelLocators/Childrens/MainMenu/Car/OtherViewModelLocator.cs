using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Other;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Car
{
    public class OtherViewModelLocator : ViewModelLocatorBase
    {
        public EquipmentViewModel Equipment
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new EquipmentViewModel(null, null);
                }
                else
                {
                    return App.Current.GetService<EquipmentViewModel>();
                }
            }
        }

        public EquipmentListViewModel EquipmentList
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new EquipmentListViewModel(null, null);
                }
                else
                {
                    return App.Current.GetService<EquipmentListViewModel>();
                }
            }
        }
    }
}