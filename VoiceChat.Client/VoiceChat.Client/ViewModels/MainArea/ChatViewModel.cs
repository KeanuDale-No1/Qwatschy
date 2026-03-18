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
        public ObservableCollection<ChatMessage> Messages { get; set; } = new();
        [ObservableProperty] private string messageInput = "";

        private readonly ServiceHubClient chatService;
        private readonly AppState appState;
        public event Action? MessageAdded;
        public ChatViewModel(ServiceHubClient chatService, AppState appState)
        {
            this.appState = appState;
            this.chatService = chatService;
            chatService.MessageReceived += OnMessageReceived;
        }


        private void OnMessageReceived(ChatMessageDTO message)
        {
            Messages.Add(new ChatMessage(message.Username, message.Text, message.Timestamp,appState.GetUser().ClientId == message.ClientId ? HorizontalAlignment.Right : HorizontalAlignment.Left, appState.GetUser().ClientId == message.ClientId,message.Username.Substring(0, message.Username.Length > 2 ? 2:message.Username.Length)));
            MessageAdded?.Invoke();
        }

        [RelayCommand]
        private async Task SendMessage()
        {
            if (chatService.Connection?.State == HubConnectionState.Connected && String.IsNullOrWhiteSpace(MessageInput) == false)
            {
                await chatService.SendMessage(new ChatMessageDTO() { ClientId = appState.GetUser().ClientId, Username = appState.GetUser().UserName, Text = MessageInput, Timestamp = DateTime.UtcNow });
                MessageInput = "";
            }
        }

    }
}