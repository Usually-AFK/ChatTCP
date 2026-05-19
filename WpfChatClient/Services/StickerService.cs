using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WpfChatClient.Services;

public class StickerService : IStickerService
{
    private const string MarkerPrefix = "[sticker:";
    private const string MarkerSuffix = "]";
    private static readonly Regex StickerMarkerRegex = new(
        @"^\[sticker:([a-z0-9_-]+)\]$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private static readonly IReadOnlyList<StickerItem> Stickers = new List<StickerItem>
    {
        new("ok", "OK", "\u2705", "#22C55E"),
        new("fire", "Fire", "\uD83D\uDD25", "#F97316"),
        new("party", "Party", "\uD83C\uDF89", "#A855F7"),
        new("laptop", "Laptop", "\uD83D\uDCBB", "#38BDF8"),
        new("hello", "Hello", "\uD83D\uDC4B", "#F59E0B"),
        new("sad", "Sad", "\uD83D\uDE22", "#64748B")
    };

    public IReadOnlyList<StickerItem> GetBuiltInStickers()
    {
        return Stickers;
    }

    public StickerItem? TryGetSticker(string stickerId)
    {
        if (string.IsNullOrWhiteSpace(stickerId))
        {
            return null;
        }

        return Stickers.FirstOrDefault(s =>
            s.Id.Equals(stickerId.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public StickerItem? TryGetStickerFromMessage(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        var match = StickerMarkerRegex.Match(content.Trim());
        if (!match.Success)
        {
            return null;
        }

        return TryGetSticker(match.Groups[1].Value);
    }

    public string CreateStickerMessage(string stickerId)
    {
        var sticker = TryGetSticker(stickerId);
        if (sticker == null)
        {
            throw new ArgumentException("Unknown sticker id.", nameof(stickerId));
        }

        return MarkerPrefix + sticker.Id + MarkerSuffix;
    }
}
