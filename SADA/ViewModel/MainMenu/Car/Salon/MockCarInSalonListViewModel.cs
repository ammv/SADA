using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    class MockCarInSalonListViewModel: CarInSalonListViewModel
    {
        private ObservableCollection<DataLayer.Car> _mockEntities;
        public ObservableCollection<DataLayer.Car> MockEntities
        {
            get => _mockEntities;
            private set => SetProperty(ref _mockEntities, value);
        }
        public MockCarInSalonListViewModel(): base()
        {
            MockEntities = new ObservableCollection<DataLayer.Car>
            {
                new DataLayer.Car
                {
                    YearOfRelease = 2024,
                    Staff = new DataLayer.Staff
                    {
                        Passport = new DataLayer.Passport
                        {
                            Name = "Григорий",
                            Surname = "Григорьев",
                            Patronymic = "Григорьевич"

                        },
                        StaffPost = new DataLayer.StaffPost
                        {
                            Name = "Менеджер"
                        }
                    }
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
