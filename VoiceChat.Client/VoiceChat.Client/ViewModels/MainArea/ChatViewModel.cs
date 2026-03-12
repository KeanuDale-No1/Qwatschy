using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.MainArea
{
    public partial class ChatViewModel:ViewModelBase
    {
        public ObservableCollection<ChatMessageDTO> Messages { get; set; } = new();
        [ObservableProperty] private string messageInput = "";

        private readonly ChatService chatService;


        public ChatViewModel(ChatService chatService)
        {
            this.chatService = chatService;
            chatService.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(ChatMessageDTO message)
        {
            Messages.Add(message);
        }

        [RelayCommand]
        private async Task SendMessage()
        {
            if (chatService.Connection.State == HubConnectionState.Connected && String.IsNullOrWhiteSpace(messageInput.ToString())== false)
            {
                await chatService.SendMessage(new ChatMessageDTO() { Username = "Tester", Text =  messageInput.ToString(), Timestamp = DateTime.UtcNow});
                MessageInput = "";
            }
        }

    }
}