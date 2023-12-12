using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Services
{
    public interface ITabService
    {
        void OpenTab(TabObservableObject viewModel);

        void OpenTab<TTabViewModel>(string name) where TTabViewModel : TabObservableObject;

        void OpenTabForSelect<TTabListViewModel, T>(string name, Action<T> selectAction) where TTabListViewModel: TabObservableObjectList<T> where T: class, new();

        void OpenTabForSelect<TTabListViewModel, T>(string name, ICollection<T> collection, Action<T> setter) where TTabListViewModel : TabObservableObjectList<T> where T : class, new();

        /*
         * 
         * 
         * 
         * 
         * 
         * 
         *
        var vm = App.Current.GetService<Salon.CarInSalonListViewModel>();
        vm.Name = "Выбор автомобиля";
                    vm.CurrentListMode = ListMode.Select;
                    vm.SelectAction = (car) =>
                    {
                        Entity.Car = _cars.FirstOrDefault(c => c.ID ==car.ID);
                    };
    _tabService.OpenTab(vm);
                    break;
        */
    }
}
