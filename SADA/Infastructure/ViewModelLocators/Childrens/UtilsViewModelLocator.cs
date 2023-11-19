using SADA.Infastructure.Core;
using SADA.ViewModel.Utils;

namespace SADA.Infastructure.ViewModelLocators.Childrens
{
    internal class UtilsViewModelLocator : ViewModelLocatorBase
    {
        public WindowTopButtonsViewModel WindowTopButtons
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new WindowTopButtonsViewModel();
                }
                else
                {
                    return App.Current.GetService<WindowTopButtonsViewModel>();
                }
            }
        }
    }
}