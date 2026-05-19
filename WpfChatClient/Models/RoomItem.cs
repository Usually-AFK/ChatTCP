using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfChatClient.Models;

public partial class RoomItem : ObservableObject
{
    public RoomItem(string id, string displayName)
    {
        Id = id;
        DisplayName = displayName;
    }

    public string Id { get; }
    public string DisplayName { get; }

    [ObservableProperty]
    private int _unreadCount;

    public bool HasUnread => UnreadCount > 0;

    partial void OnUnreadCountChanged(int value)
    {
        OnPropertyChanged(nameof(HasUnread));
    }
}
