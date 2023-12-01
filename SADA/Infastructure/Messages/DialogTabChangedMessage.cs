using CommunityToolkit.Mvvm.Messaging.Messages;
using SADA.Infastructure.Core;

namespace SADA.Infastructure.Messages
{
    public class DialogTabChangedMessage : ValueChangedMessage<TabObservableObject>
    {
        public DialogTabChangedMessage(TabObservableObject value) : base(value)
        {
        }
    }

    //public class ThemeRequestMessage : RequestMessage<TabObservableObject>
    //{
    //}
}