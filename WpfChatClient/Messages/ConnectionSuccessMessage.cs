using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WpfChatClient.Messages;

public class ConnectionSuccessMessage : ValueChangedMessage<string>
{
    public ConnectionSuccessMessage(string username) : base(username) { }
}
