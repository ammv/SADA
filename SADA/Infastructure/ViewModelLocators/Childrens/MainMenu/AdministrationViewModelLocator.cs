using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Administration;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu
{
    public class AdministrationViewModelLocator : ViewModelLocatorBase
    {
        public SystemViewModelLocator SystemVml { get; } = new SystemViewModelLocator();
        public LoginViewModelLocator LoginVml { get; } = new LoginViewModelLocator();
    }
}