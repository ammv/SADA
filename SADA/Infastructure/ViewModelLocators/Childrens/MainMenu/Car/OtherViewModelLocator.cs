using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Car.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Car
{
    class OtherViewModelLocator: ViewModelLocatorBase
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
