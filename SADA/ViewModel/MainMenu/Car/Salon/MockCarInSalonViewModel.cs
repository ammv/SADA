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
            Cars = new System.Collections.ObjectModel.ObservableCollection<DataLayer.Car>
            {
                new DataLayer.Car
                {
                    YearOfRelease = 2024
                },
                new DataLayer.Car
                {
                    YearOfRelease = 2021
                },
                new DataLayer.Car
                {
                    YearOfRelease = 2026
                }
            };
        }
    }
}
