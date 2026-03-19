using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Hubs;
public class ServiceHubClient(TokenService tokenService, StateService stateService) : IDisposable
{
    public HubConnection Connection { get; private set; }
    
    public event Action<ChatMessageDTO>? MessageReceived;
    public event Action<ChannelDTO>? ChannelAdd;
    public event Action<Guid>? ChannelRemove;

    public event Action<ConnectChannelResponseDTO>? UserJoinChannel;
    public event Action<byte[]>? OnReceiveAudioFrame;
    public async Task Connect()
    {
        try
        {
            string url = stateService.ServerAddress.TrimEnd("/").ToString() + "/connection";

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

            Connection.On<string, byte[]>("ReceiveAudioFrame", (connectionId, opusChunk) =>
            {
                OnReceiveAudioFrame?.Invoke(opusChunk);
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
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async Task connection_Reconnected(string? arg) {}

    private async Task connection_Reconnecting(Exception? exception) {}

    private async Task Connection_Closed(Exception? exception) { }

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

    internal void SendAudioFrame(Guid channelId, byte[] opusData)
    {
        Connection.InvokeAsync("SendAudioFrame",channelId, opusData);
    }
}