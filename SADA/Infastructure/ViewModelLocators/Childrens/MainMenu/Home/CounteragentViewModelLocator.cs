using SADA.Infastructure.Core;
using SADA.ViewModel.MainMenu.Home.Counteragent;
using SADA.ViewModel.MainMenu.Home.Counteragent.Mocks;

namespace SADA.Infastructure.ViewModelLocators.Childrens.MainMenu.Home
{
    public class CounteragentViewModelLocator : ViewModelLocatorBase
    {
        public CounteragentViewModel Counteragent
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockCounteragentViewModel();
                }
                else
                {
                    return App.Current.GetService<CounteragentViewModel>();
                }
            }
        }

        public CounteragentListViewModel CounteragentList
        {
            get
            {
                if (IsInDesignMode())
                {
                    return new MockCounteragentListViewModel();
                }
                else
                {
                    return App.Current.GetService<CounteragentListViewModel>();
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