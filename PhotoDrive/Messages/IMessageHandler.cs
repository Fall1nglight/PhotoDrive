using CommunityToolkit.Mvvm.Messaging;

namespace PhotoDrive.Messages
{
    public interface IMessageHandler<TMessage> : IRecipient<TMessage>
        where TMessage : class, IBaseMessage
    {
        void SendMessage(TMessage message);
    }
}
