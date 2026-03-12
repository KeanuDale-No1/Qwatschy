using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Services;

public class ChatService
{

    public HubConnection Connection { get; private set; }
    AppState appState;

    public event Action<ChatMessageDTO>? MessageReceived;
    public ChatService(AppState appState)
    {

        this.appState = appState;
        
        
    }
    public async Task Connect ()
    {
        string url = appState.ServerAdress.TrimEnd("/").ToString() + "/chat";
        
        Connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        Connection.On<ChatMessageDTO>("ReceiveMessage", (chatmessage) =>
        {
            MessageReceived?.Invoke(chatmessage);
        });
        await Connection.StartAsync();
    }


    public Task SendMessage(ChatMessageDTO message) => 
            Connection.InvokeAsync("SendMessage", message);

}
