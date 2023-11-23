using SADA.Infastructure.Core;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.ViewModel.MainMenu.Car.Salon
{
    class CarInSalonFormViewModel: TabObservableObjectForm<DataLayer.Car>
    {
        private readonly IDialogService _dialogService;

        public CarInSalonFormViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        protected CarInSalonFormViewModel() { }
    }
}
