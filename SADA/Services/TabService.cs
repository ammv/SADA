using CommunityToolkit.Mvvm.Messaging;
using SADA.Infastructure.Core;
using SADA.Infastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Services
{
    public class TabService : ITabService
    {
        public void OpenTab<TTabViewModel>(string name = null) where TTabViewModel : TabObservableObject
        {
            TabObservableObject tabObservableObject = App.Current.GetService<TTabViewModel>();

            tabObservableObject.Name = name;

            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(tabObservableObject));
        }

        public void OpenTab(TabObservableObject viewModel)
        {
            WeakReferenceMessenger.Default.Send(new DialogTabChangedMessage(viewModel));
        }

        public void OpenTabForSelect<TTabListViewModel, T>(string name, Action<T> selectAction)
            where TTabListViewModel : TabObservableObjectList<T>
            where T : class, new()
        {
            _OpenTabForSelect<TTabListViewModel, T>(name, selectAction);
        }

        public void OpenTabForSelect<TTabListViewModel, T>(string name, ICollection<T> collection, Action<T> setter)
            where TTabListViewModel : TabObservableObjectList<T>
            where T : class, new()
        {

            Func<T, T, bool> equaler = (T instance, T instance2) =>
            {
                Type type = typeof(T);
                var idProperties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Where(p => p.Name.EndsWith("ID")).ToList();

                if (idProperties == null || idProperties.Count() == 0) return Equals(instance, instance2);

                foreach(var prop in idProperties)
                {
                    if(!Equals(prop.GetValue(instance), prop.GetValue(instance2)))
                    {
                        return false;
                    }
                }

                return true;      
            };
            Action<T> selectAction = (T entity) =>
            {
                T foundEntity = collection.FirstOrDefault(item => equaler(item, entity));
                if (foundEntity == null) collection.Add(entity);
                setter(foundEntity);
            };

            _OpenTabForSelect<TTabListViewModel, T>(name, selectAction);
        }

        private void _OpenTabForSelect<TTabListViewModel, T>(string name, Action<T> selectAction)
            where TTabListViewModel : TabObservableObjectList<T>
            where T : class, new()
        {
            TTabListViewModel tabObservableObjectList = App.Current.GetService<TTabListViewModel>();
            tabObservableObjectList.Name = name;
            tabObservableObjectList.CurrentListMode = Infastructure.Core.Enums.ListMode.Select;
            tabObservableObjectList.SelectAction = selectAction;

            OpenTab(tabObservableObjectList);
        }
    }
}
