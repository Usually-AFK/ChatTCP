using System.Collections.Generic;

namespace WpfChatClient.Services;

public record StickerItem(
    string Id,
    string DisplayName,
    string Glyph,
    string AccentColor);

public interface IStickerService
{
    IReadOnlyList<StickerItem> GetBuiltInStickers();
    StickerItem? TryGetSticker(string stickerId);
    StickerItem? TryGetStickerFromMessage(string content);
    string CreateStickerMessage(string stickerId);
}
