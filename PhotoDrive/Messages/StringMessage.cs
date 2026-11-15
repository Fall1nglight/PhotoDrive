using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PhotoDrive.Messages
{
    public class StringMessage : ValueChangedMessage<string>, IBaseMessage
    {
        public string Title { get; private set; }

        public StringMessage(string title, string value) : base(value)
        {
            Title = title;
        }
    }
}
