using System;
using System.Threading;
using System.Threading.Tasks;
using VoiceChat.Shared.Networking;

namespace VoiceChat.Shared.Networking;

public interface IWebSocketService
{
    Task ConnectAsync(Uri uri, CancellationToken ct = default);
    Task SendAsync(SocketMessage msg, CancellationToken ct = default);
    Task DisconnectAsync();
    event Action<SocketMessage>? OnMessage;
    bool IsConnected { get; }
}
