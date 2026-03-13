using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Services;

public class ChatService(AppState appState, StatusService statusService) : IDisposable
{
    
    private readonly AppState appState = appState;
    private readonly StatusService statusService = statusService;
    public HubConnection Connection { get; private set; }
    

    public event Action<ChatMessageDTO>? MessageReceived;

    public async Task Connect (string serveradress)
    {
        statusService.AddReport("baue verbindung zum Chat auf.");

        string url = serveradress.TrimEnd("/").ToString() + "/chat";

        Connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        Connection.On<ChatMessageDTO>("ReceiveMessage", (chatmessage) =>
        {
            MessageReceived?.Invoke(chatmessage);
        });
        Connection.Closed += Connection_Closed;
        Connection.Reconnecting += connection_Reconnecting;
        Connection.Reconnected += connection_Reconnected;
        await Connection.StartAsync();
        statusService.AddReport("Verbindung zum Chat aufgebaut.");
    }

    private async Task connection_Reconnected(string? arg) => 
        statusService.AddReport($"Verbindung zum Chat wurde wieder hergestellt");


    private async Task connection_Reconnecting(Exception? exception) => 
        statusService.AddReport($"Verbindung zum Chat verloren. Versuche erneut zu verbinden... {exception?.Message}");

    private async Task Connection_Closed(Exception? exception) => 
        statusService.AddReport($"Verbindung wurde geschlossen... {exception?.Message}");
    
    

    public Task SendMessage(ChatMessageDTO message) => 
            Connection.InvokeAsync("SendMessage", message);

    public void Dispose()
    {
        Connection.Closed -= Connection_Closed;
        Connection.Reconnecting -= connection_Reconnecting;
        Connection.Reconnected -= connection_Reconnected;
    }
}
