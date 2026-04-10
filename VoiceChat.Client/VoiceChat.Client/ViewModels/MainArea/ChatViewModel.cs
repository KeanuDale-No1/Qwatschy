using Avalonia.Controls;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.MainArea
{

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

        private readonly ChatHubClient chatService;
        private readonly IAppSettingsService appState;
        private readonly ChannelService channelService;
        public event Action? MessageAdded;
        public event Action? ScrollToBottom;
        private readonly StateService stateService;
        
        private int loadedCount = 0;
        private const int InitialLoad = 40;
        private const int LoadMoreCount = 50;

        public ChatViewModel(ChatHubClient chatService, IAppSettingsService appState, StateService stateService, ChannelService channelService)
        {
            this.stateService = stateService;
            this.appState = appState;
            this.chatService = chatService;
            this.channelService = channelService;
            chatService.MessageReceived += OnMessageReceived;
            stateService.SelectedChannelChanged += OnSelectedChannelChanged;
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

        private void OnSelectedChannelChanged(Guid? channelId)
        {
            IsInputEnabled = stateService.SelectChannelId.HasValue;
            if (stateService.SelectChannelId.HasValue)
            {
                var channel = channelService.Channels.FirstOrDefault(c => c.Id == stateService.SelectChannelId.Value);
                ChannelTitle = channel?.Name ?? "";
                _ = LoadMessagesForChannel(stateService.SelectChannelId.Value);
            }
            else
            {
                ChannelTitle = "";
                Messages.Clear();
            }
        }

        private async Task LoadMessagesForChannel(Guid channelId)
        {
            try
            {
                IsLoading = true;
                HasMoreMessages = true;
                loadedCount = 0;
                
                Debug.WriteLine($"[Chat] Loading messages for channel {channelId}");
                var response = await chatService.GetMessages(channelId, 0, InitialLoad);
                Debug.WriteLine($"[Chat] Received {response?.Messages?.Count} messages, Total: {response?.TotalCount}");
                
                Messages.Clear();
                
                foreach (var msg in response.Messages)
                {
                    Messages.Add(await CreateChatMessage(msg));
                }
                
                loadedCount = response.Messages.Count;
                HasMoreMessages = loadedCount >= InitialLoad && loadedCount < response.TotalCount;
                
                IsLoading = false;
                Debug.WriteLine($"[Chat] Messages added to collection. Count: {Messages.Count}");
                MessageAdded?.Invoke();
                ScrollToBottom?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Chat] Error loading messages: {ex.Message}");
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task LoadMoreMessages()
        {
            if (IsLoadingMore || !HasMoreMessages ||  !stateService.SelectChannelId.HasValue|| stateService.SelectChannelId == Guid.Empty)
                return;

            try
            {
                IsLoadingMore = true;
                
                var skip = loadedCount;
                var response = await chatService.GetMessages(stateService.SelectChannelId.Value, skip, LoadMoreCount);
                foreach (var msg in response.Messages)
                {
                    Messages.Insert(0, await CreateChatMessage(msg));
                }
                
                loadedCount += response.Messages.Count;
                HasMoreMessages = response.Messages.Count >= LoadMoreCount && loadedCount < response.TotalCount;
                
                IsLoadingMore = false;
            }
            catch (Exception)
            {
                IsLoadingMore = false;
            }
        }

        private async Task<ChatMessage> CreateChatMessage(ChatMessageDTO msg)
        {
            
            var username = msg.Username ?? "Unknown";
            var initials = username.Length > 2 ? username.Substring(0, 2) : username;
            return new ChatMessage(
                msg.ChannelId,
                username,
                msg.Content,
                msg.Timestamp,
                appState.AppSetting.UserSettings.UserId== msg.SenderId ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                appState.AppSetting.UserSettings.UserId == msg.SenderId,
                initials);
        }

        private async void OnMessageReceived(ChatMessageDTO message)
        {
            Debug.WriteLine($"[Chat] MessageReceived: Channel={message.ChannelId}, Content={message.Content}");
            Debug.WriteLine($"[Chat] Current Channel: {stateService.SelectChannelId}");
            
            if (stateService.SelectChannelId.HasValue && message.ChannelId == stateService.SelectChannelId.Value)
            {
                Debug.WriteLine($"[Chat] Adding message to collection");
                Messages.Add(await CreateChatMessage(message));
                MessageAdded?.Invoke();
                ScrollToBottom?.Invoke();
            }
            else
            {
                channelService.MarkChannelAsUnread(message.ChannelId);
            }
        }

        [RelayCommand]
        private async Task SendMessage()
        {
            Debug.WriteLine($"[Chat] SendMessage called. Connected: {chatService.Connection?.State}");
            if (chatService.Connection?.State == HubConnectionState.Connected && stateService.SelectChannelId.HasValue && String.IsNullOrWhiteSpace(MessageInput) == false)
            {
                Debug.WriteLine($"[Chat] Sending message to channel {stateService.SelectChannelId}");
                
                await chatService.SendMessage(new ChatMessageDTO(appState.AppSetting.UserSettings.UserId, stateService.SelectChannelId.Value, MessageInput, DateTime.UtcNow));
                MessageInput = "";
            }
        }
    }
}
