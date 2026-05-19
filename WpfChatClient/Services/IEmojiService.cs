using System.Collections.Generic;

namespace WpfChatClient.Services;

public class Emoji
{
    public string Unicode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public interface IEmojiService
{
    IEnumerable<Emoji> GetAllEmojis();
    IEnumerable<Emoji> GetEmojisByCategory(string category);
    IEnumerable<Emoji> SearchEmojis(string searchTerm);
    IEnumerable<string> GetCategories();
}
