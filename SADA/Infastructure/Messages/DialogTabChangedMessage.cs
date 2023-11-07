using CommunityToolkit.Mvvm.Messaging.Messages;
using SADA.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADA.Infastructure.Messages
{
    class DialogTabChangedMessage : ValueChangedMessage<TabObservableObject>
    {
        public DialogTabChangedMessage(TabObservableObject value) : base(value)
        {
        }
    }

    //public class ThemeRequestMessage : RequestMessage<TabObservableObject>
    //{
    //}
}
