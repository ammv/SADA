using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Home.Counteragent;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Home
{
    internal class CounteragentViewModelLocator : ViewModelLocatorBase
    {
        public CounteragentViewModel Counteragent
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new CounteragentViewModel();
                }
                else
                {
                    return App.Current.GetService<CounteragentViewModel>();
                }
            }
        }

        public InteractionWithCounteragentViewModel InteractionWithCounteragent
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new InteractionWithCounteragentViewModel();
                }
                else
                {
                    return App.Current.GetService<InteractionWithCounteragentViewModel>();
                }
            }
        }
    }
}