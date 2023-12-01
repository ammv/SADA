using SADA.Infastructure.Core;

namespace SADA.Infastructure.ViewModelLocators.Childrens
{
    public class DialogsViewModelLocator : ViewModelLocatorBase
    {
        public Dialogs.MainMenuViewModelLocator MainMenuVml { get; } = new Dialogs.MainMenuViewModelLocator();
        public Dialogs.OtherViewModelLocator OtherVml { get; } = new Dialogs.OtherViewModelLocator();
    }
}