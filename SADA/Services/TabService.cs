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
    }
}
