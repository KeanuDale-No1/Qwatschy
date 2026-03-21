using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.MainArea
{


    public record ChatMessage (string Username, string Text, DateTime Timestamp, HorizontalAlignment HorizontalAlignment, bool IsOwnMessage, string SenderInitial);

    public partial class ChatViewModel:ViewModelBase
    {
        public ObservableCollection<ChatMessage> Messages { get; set; } = new() { new ChatMessage("Keanu","Das ist ein Test Text", new DateTime(2026,03,19),HorizontalAlignment.Right, true ,"KD"),
                                                                                  new ChatMessage("o7","Das ist ein Test Text", new DateTime(2026,03,19),HorizontalAlignment.Left, false ,"O7")};
        [ObservableProperty] private string messageInput = "";
        [ObservableProperty] private bool isInputEnabled = false;
        [ObservableProperty] private string channelTitle = "";

        private readonly ServiceHubClient chatService;
        private readonly AppState appState;
        private readonly ChannelService channelService;
        public event Action? MessageAdded;
        private readonly StateService stateService;
        public ChatViewModel(ServiceHubClient chatService, AppState appState, StateService stateService, ChannelService channelService)
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

            Messages = new() { new ChatMessage("Keanu","Das ist ein Test Text", new DateTime(2026,03,19),HorizontalAlignment.Right, true ,"KD"),
                                                                                  new ChatMessage("o7","Das ist ein Test Text", new DateTime(2026,03,19),HorizontalAlignment.Left, false ,"O7")};
        }

        private void OnSelectedChannelChanged(Guid? channelId)
        {
            IsInputEnabled = stateService.SelectChannelId.HasValue;
            if (stateService.SelectChannelId.HasValue)
            {
                var channel = channelService.Channels.FirstOrDefault(c => c.Id == stateService.SelectChannelId.Value);
                ChannelTitle = channel?.Name ?? "";
            }
            else
            {
                ChannelTitle = "";
            }
        }

        


        private void OnMessageReceived(ChatMessageDTO message)
        {
            Messages.Add(new ChatMessage(message.Username, message.Content, message.Timestamp,appState.GetUser().ClientId == message.SenderId ? HorizontalAlignment.Right : HorizontalAlignment.Left, appState.GetUser().ClientId == message.SenderId, message.Username.Substring(0, message.Username.Length > 2 ? 2:message.Username.Length)));
            MessageAdded?.Invoke();
        }

        [RelayCommand]
        private async Task SendMessage()
        {
            if (chatService.Connection?.State == HubConnectionState.Connected && stateService.SelectChannelId.HasValue && String.IsNullOrWhiteSpace(MessageInput) == false)
            {
                await chatService.SendMessage(new ChatMessageDTO(appState.GetUser().ClientId, stateService.SelectChannelId.Value, MessageInput, DateTime.UtcNow));
                MessageInput = "";
            }
        }

    }
}