using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    class MockCarInSalonViewModel: CarInSalonViewModel
    {
        public MockCarInSalonViewModel(): base()
        {
            Entity = new DataLayer.Car
            {
                VIN = "13r2fe3fwefwr"
            };
        }
    }
}
