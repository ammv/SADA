using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.Childrens.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.ViewModelLocators.Childrens
{
    class MainMenuViewModelLocator: ViewModelLocatorBase
    {

        public AdministrationViewModelLocator AdministationVml { get; } = new AdministrationViewModelLocator();
        public CarViewModelLocator CarVml { get; } = new CarViewModelLocator();
        public ProductViewModelLocator ProductVml { get; } = new ProductViewModelLocator();
        public ManualViewModelLocator ManualVml { get; } = new ManualViewModelLocator();
        public HomeViewModelLocator HomeVml { get; } = new HomeViewModelLocator();
        public SalaryAndStaffViewModelLocator SalaryAndStaffVml { get; } = new SalaryAndStaffViewModelLocator();

    }
}
