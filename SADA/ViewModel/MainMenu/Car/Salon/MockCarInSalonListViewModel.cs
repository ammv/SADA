using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    class MockCarInSalonListViewModel: CarInSalonListViewModel
    {
        public MockCarInSalonListViewModel(): base()
        {
            Entities = new System.Collections.ObjectModel.ObservableCollection<DataLayer.Car>
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
