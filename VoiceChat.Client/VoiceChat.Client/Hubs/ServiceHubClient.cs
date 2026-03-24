using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
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
                .WithUrl(url, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(tokenService.ReadToken());
                })
                
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions = Json.Options;
                })
                .WithAutomaticReconnect()
                .Build();

            Connection.On<ChatMessageDTO>("ReceiveMessage", (chatmessage) =>
            {
                try { MessageReceived?.Invoke(chatmessage); } catch { }
            });

            Connection.On<ChannelDTO>("AddChannelChange", (channel) =>
            {
                try { ChannelAdd?.Invoke(channel); } catch { }
            });

            Connection.On<Guid>("DeleteChannelChange", (channel) =>
            {
                try { ChannelRemove?.Invoke(channel); } catch { }
            });

            Connection.On<ConnectChannelResponseDTO>("JoinChannel", (channel) =>
            {
                try { UserJoinChannel?.Invoke(channel); } catch { }
            });

            Connection.On<string, byte[]>("ReceiveAudioFrame", (connectionId, opusChunk) =>
            {
                try { OnReceiveAudioFrame?.Invoke(opusChunk); } catch { }
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
                Console.WriteLine($"Fehler bei Connection.StartAsync {ex}");
            }
        }
        catch (Exception)
        {
        }
    }
    public async Task Disconnect()
    {
        if (Connection == null) return;
        await Connection.StopAsync();
        await Connection.DisposeAsync();
    }
    private async Task connection_Reconnected(string? arg) { }

    private async Task connection_Reconnecting(Exception? exception) { Console.WriteLine("connection_Reconnecting: " + exception??""); }

    private async Task Connection_Closed(Exception? exception) { Console.WriteLine("Connection_Closed: " + exception??""); }

    public Task SendMessage(ChatMessageDTO message)
    {
        if (Connection == null) return Task.CompletedTask;
        return Connection.InvokeAsync("SendMessage", message);
    }

    public Task AddChannel(ChannelDTO channel)
    {
        if (Connection == null) return Task.CompletedTask;
        return Connection.InvokeAsync("AddChannel", channel);
    }

    public Task DeleteChannel(Guid channelId)
    {
        if (Connection == null) return Task.CompletedTask;
        return Connection.InvokeAsync("DeleteChannel", channelId);
    }

    public Task JoinChannel(Guid guid)
    {
        if (Connection == null) return Task.CompletedTask;
        return Connection.InvokeAsync("JoinChannel", guid);
    }

    public void Dispose()
    {
        Connection.Closed -= Connection_Closed;
        Connection.Reconnecting -= connection_Reconnecting;
        Connection.Reconnected -= connection_Reconnected;
    }

    internal void SendAudioFrame(Guid channelId, byte[] opusData)
    {
        if (Connection == null) return;
        _ = Connection.InvokeAsync("SendAudioFrame", channelId, opusData);
    }

    public Task KickUser(Guid channelId, Guid userId)
    {
        if (Connection == null) return Task.CompletedTask;
        return Connection.InvokeAsync("KickUser", channelId, userId);
    }

    public Task BanUser(Guid channelId, Guid userId)
    {
        if (Connection == null) return Task.CompletedTask;
        return Connection.InvokeAsync("BanUser", channelId, userId);
    }

    public async Task<GetMessagesResponseDTO> GetMessages(Guid channelId, int skip = 0, int take = 40)
    {
        if (Connection == null) return new GetMessagesResponseDTO(new List<ChatMessageDTO>(), 0);
          var response = await Connection.InvokeAsync<GetMessagesResponseDTO>("GetMessages", channelId, skip, take);
        return response;

    }
}