using CommunityToolkit.Mvvm.ComponentModel;

namespace SADA.Services
{
    public interface INavigationService
    {
        ObservableObject CurrentView { get; }

        void NavigateTo<T>() where T : ObservableObject;
    }
}
