using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Hubs;

public interface IClientHubExchange
{
    Task ConnectAsync(Guid serverId, string serverAddress);
    Task DisconnectAsync(Guid serverId);
    Task<GetMessagesResponseDTO> GetMessagesAsync(Guid serverId, Guid channelId, int skip = 0, int take = 50);
    Task SendMessageAsync(Guid serverId, ChatMessageDTO message);
    Task AddServerAsync(string serverAddress);
    Task ConnectAllAsync();
}

public enum ServerConnectionState
{
    Disconnected,
    Connecting,
    Connected,
    Reconnecting,
    Reconnected,
    Error
}