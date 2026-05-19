using System;

namespace WpfChatClient.Models;

public record ToastNotification(
    string Id,
    string Title,
    string Message,
    string Source,
    string Kind,
    string Time);
