using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfChatClient.Services;

public class EmojiService : IEmojiService
{
    private static readonly List<Emoji> _emojis = new()
    {
        new Emoji { Unicode = "😀", Name = "grinning face", Category = "Smileys & Emotion" },
        new Emoji { Unicode = "😂", Name = "face with tears of joy", Category = "Smileys & Emotion" },
        new Emoji { Unicode = "❤️", Name = "red heart", Category = "Smileys & Emotion" },
        new Emoji { Unicode = "👍", Name = "thumbs up", Category = "People & Body" },
        new Emoji { Unicode = "🎉", Name = "party popper", Category = "Activities" },
        new Emoji { Unicode = "🔥", Name = "fire", Category = "Travel & Places" },
        new Emoji { Unicode = "🚀", Name = "rocket", Category = "Travel & Places" },
        new Emoji { Unicode = "💻", Name = "laptop", Category = "Objects" },
        new Emoji { Unicode = "🍕", Name = "pizza", Category = "Food & Drink" },
        new Emoji { Unicode = "🐶", Name = "dog face", Category = "Animals & Nature" }
    };

    public IEnumerable<Emoji> GetAllEmojis()
    {
        return _emojis;
    }

    public IEnumerable<Emoji> GetEmojisByCategory(string category)
    {
        return _emojis.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Emoji> SearchEmojis(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return _emojis;
        }

        return _emojis.Where(e => e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<string> GetCategories()
    {
        return _emojis.Select(e => e.Category).Distinct();
    }
}
