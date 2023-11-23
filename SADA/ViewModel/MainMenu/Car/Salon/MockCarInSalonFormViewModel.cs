using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    class MockCarInSalonFormViewModel: CarInSalonFormViewModel
    {
        public MockCarInSalonFormViewModel(): base()
        {
            Entity = new DataLayer.Car
            {
                VIN = "13r2fe3fwefwr"
            };
        }
    }
}
