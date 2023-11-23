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
    }
}
