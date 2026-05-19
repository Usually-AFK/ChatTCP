using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfChatClient.Core.Interfaces;
using WpfChatClient.Messages;
using System.Threading.Tasks;

namespace WpfChatClient.ViewModels;

public partial class ConnectViewModel : ObservableObject
{
    private readonly IChatService _chatService;

    [ObservableProperty]
    private string _ip = "127.0.0.1";

    [ObservableProperty]
    private string _username = "";

    [ObservableProperty]
    private string _errorMessage = "";

    public ConnectViewModel(IChatService chatService)
    {
        _chatService = chatService;
    }

    [RelayCommand]
    private async Task Connect()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "Username is required.";
            return;
        }

        var serverIp = string.IsNullOrWhiteSpace(Ip) ? "127.0.0.1" : Ip.Trim();

        try
        {
            ErrorMessage = "";
            await _chatService.ConnectAsync(serverIp, 5000, Username.Trim());
            
            // Signal successful connection
            WeakReferenceMessenger.Default.Send(new ConnectionSuccessMessage(Username.Trim()));
        }
        catch (System.Net.Sockets.SocketException)
        {
            ErrorMessage = "Cannot connect. Check server IP, Wi-Fi, firewall, and that ChatServer is running.";
        }
        catch (System.TimeoutException)
        {
            ErrorMessage = "Connection timed out. Check the host IP and Wi-Fi network.";
        }
        catch (System.InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (System.Exception ex)
        {
            ErrorMessage = $"Connection failed: {ex.Message}";
        }
    }
}
