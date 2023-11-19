using CommunityToolkit.Mvvm.Messaging.Messages;
using SADA.Infastructure.Core;

namespace SADA.Infastructure.Messages
{
    internal class DialogTabChangedMessage : ValueChangedMessage<TabObservableObject>
    {
        public DialogTabChangedMessage(TabObservableObject value) : base(value)
        {
        }
    }

    //public class ThemeRequestMessage : RequestMessage<TabObservableObject>
    //{
    //}
}