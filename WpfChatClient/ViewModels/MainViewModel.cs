using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using WpfChatClient.Messages;

namespace WpfChatClient.ViewModels;

public partial class MainViewModel : ObservableObject, IRecipient<ConnectionSuccessMessage>
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ObservableObject _currentView;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _currentView = _serviceProvider.GetRequiredService<ConnectViewModel>();

        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(ConnectionSuccessMessage message)
    {
        try
        {
            var dispatcher = System.Windows.Application.Current?.Dispatcher;
            void SwitchToChatView()
            {
                // Navigate to ChatViewModel on successful connection
                var chatVM = _serviceProvider.GetRequiredService<ChatViewModel>();
                CurrentView = chatVM;
            }

            if (dispatcher == null)
            {
                SwitchToChatView();
            }
            else
            {
                dispatcher.BeginInvoke((Action)SwitchToChatView);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MAIN] Error switching to chat view: {ex.Message}");
            // Handle error (e.g., stay on connect view or show message)
        }
    }
}
