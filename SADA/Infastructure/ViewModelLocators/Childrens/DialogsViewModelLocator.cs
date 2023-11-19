using SADA.Infastructure.Core;

namespace SADA.Infastructure.ViewModelLocators.Childrens
{
    internal class DialogsViewModelLocator : ViewModelLocatorBase
    {
        public Dialogs.MainMenuViewModelLocator MainMenuVml { get; } = new Dialogs.MainMenuViewModelLocator();
    }
}