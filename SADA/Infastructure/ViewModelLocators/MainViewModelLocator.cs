using SADA.Infastructure.Core;
using SADA.Infastructure.ViewModelLocators.Childrens;

namespace SADA.Infastructure.ViewModelLocators
{
    internal class MainViewModelLocator : ViewModelLocatorBase
    {
        public StartViewModelLocator StartVml { get; } = new StartViewModelLocator();
        public MainMenuViewModelLocator MainMenuVml { get; } = new MainMenuViewModelLocator();
        public DialogsViewModelLocator DialogsVml { get; } = new DialogsViewModelLocator();
        public UtilsViewModelLocator UtilsVml { get; } = new UtilsViewModelLocator();
    }
}