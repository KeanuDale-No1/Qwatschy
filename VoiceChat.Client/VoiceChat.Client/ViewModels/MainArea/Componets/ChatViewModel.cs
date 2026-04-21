using Avalonia.Controls;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;


public record ChatMessage(Guid ChannelId, string Username, string Text, DateTime Timestamp, HorizontalAlignment HorizontalAlignment, bool IsOwnMessage, string SenderInitial);

public partial class ChatViewModel : ViewModelBase
{
    public ObservableCollection<ChatMessage> Messages { get; set; } = new();

    [ObservableProperty] private string messageInput = "";
    [ObservableProperty] private bool isInputEnabled = false;
    [ObservableProperty] private string channelTitle = "";
    [ObservableProperty] private bool isLoading = false;
    [ObservableProperty] private bool hasMoreMessages = true;
    [ObservableProperty] private bool isLoadingMore = false;

    private readonly IAppSettingsService appState;
    public event Action? MessageAdded;
    public event Action? ScrollToBottom;
    
    private int loadedCount = 0;
    private const int InitialLoad = 40;
    private const int LoadMoreCount = 50;

    public ChatViewModel( IAppSettingsService appState, StateService stateService)
    {
        this.appState = appState;
    }

    public ChatViewModel()
    {
        if (!Design.IsDesignMode)
            throw new InvalidOperationException(
                "Parameterloser Konstruktor darf nur im Designer verwendet werden.");

        Messages = new()
        {
            new ChatMessage(Guid.Empty, "Keanu", "Das ist ein Test Text", new DateTime(2026, 03, 19), HorizontalAlignment.Right, true, "KD"),
            new ChatMessage(Guid.Empty, "o7", "Das ist ein Test Text", new DateTime(2026, 03, 19), HorizontalAlignment.Left, false, "O7")
        };
    }



    [RelayCommand]
    public async Task LoadMoreMessages()
    {
        if (IsLoadingMore || !HasMoreMessages )
            return;

        try
        {
            IsLoadingMore = true;
            
            var skip = loadedCount;
            IsLoadingMore = false;
        }
        catch (Exception)
        {
            IsLoadingMore = false;
        }
    }


   
    [RelayCommand]
    private async Task SendMessage()
    {
        Debug.WriteLine($"[Chat] SendMessage called. Connected: ");
    }
}
