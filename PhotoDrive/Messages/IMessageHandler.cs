using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.Messages
{
    public interface IMessageHandler<TMessage> : IRecipient<TMessage>
        where TMessage : class, IBaseMessage
    {
        void SendMessage(TMessage message);
    }
}
