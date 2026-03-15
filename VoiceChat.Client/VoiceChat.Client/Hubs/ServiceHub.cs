using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Hubs;

public class ServiceHub(StatusService statusService, TokenService tokenService) : IDisposable
{
    
    private readonly StatusService statusService = statusService;
    public HubConnection Connection { get; private set; }
    

    public event Action<ChatMessageDTO>? MessageReceived;
    public event Action<ChannelDTO>? ChannelAdd;
    public event Action<Guid>? ChannelRemove;


    public event Action<ConnectChannelResponseDTO>? UserJoinChannel;

    public async Task Connect (string serveradress)
    {
        try
        {
            statusService.AddReport("baue verbindung zum Server auf.");

            string url = serveradress.TrimEnd("/").ToString() + "/connection";

            Connection = new HubConnectionBuilder()
                //.WithUrl(url)
                .WithUrl(url, options => { options.AccessTokenProvider = () => Task.FromResult(tokenService.ReadToken()); })
                .WithAutomaticReconnect()
                .Build();

            Connection.On<ChatMessageDTO>("ReceiveMessage", (chatmessage) =>
            {
                MessageReceived?.Invoke(chatmessage);
            });

            Connection.On<ChannelDTO>("AddChannelChange", (channel) =>
            {
                ChannelAdd?.Invoke(channel);
            });

            Connection.On<Guid>("DeleteChannelChange", (channel) =>
            {
                ChannelRemove?.Invoke(channel);
            });

            Connection.On<ConnectChannelResponseDTO>("JoinChannel", (channel) =>
            {
                UserJoinChannel?.Invoke(channel);
            });

            Connection.Closed += Connection_Closed;
            Connection.Reconnecting += connection_Reconnecting;
            Connection.Reconnected += connection_Reconnected;

            try
            {
                await Connection.StartAsync();
            }
            catch (Exception ex)
            {
                statusService.AddReport("SignalR Fehler: " + ex.Message);
                return;
            }
            statusService.AddReport("Verbindung zum Server aufgebaut.");
        }
        catch (Exception ex)
        {
            statusService.AddReport(ex.Message);
        }
    }

    private async Task connection_Reconnected(string? arg) => 
        statusService.AddReport($"Verbindung zum Server wurde wieder hergestellt");


    private async Task connection_Reconnecting(Exception? exception) => 
        statusService.AddReport($"Verbindung zum Server verloren. Versuche erneut zu verbinden... {exception?.Message}");

    private async Task Connection_Closed(Exception? exception) => 
        statusService.AddReport($"Verbindung wurde geschlossen... {exception?.Message}");
    
    

    public Task SendMessage(ChatMessageDTO message) => 
            Connection.InvokeAsync("SendMessage", message);

    public Task AddChannel(ChannelDTO channel) =>
        Connection.InvokeAsync("AddChannel", channel);

    public Task DeleteChannel(Guid channelId) =>
        Connection.InvokeAsync("DeleteChannel", channelId);

    public Task JoinChannel(Guid guid) =>
        Connection.InvokeAsync("JoinChannel", guid);

    public void Dispose()
    {
        Connection.Closed -= Connection_Closed;
        Connection.Reconnecting -= connection_Reconnecting;
        Connection.Reconnected -= connection_Reconnected;
    }
}
