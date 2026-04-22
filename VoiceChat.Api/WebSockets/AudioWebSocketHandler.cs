using System.Collections.Concurrent;
using System.Net.WebSockets;
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Api.WebSockets;

public record WebSocketConnection(WebSocket Socket, Guid UserId, string Username);

public class AudioWebSocketHandler
{
    public static readonly ConcurrentDictionary<string, List<WebSocketConnection>> ConnectedUsers = new();

    public async Task HandleWebSocketAsync(string? channelId, WebSocket socket, Guid userId, string username, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(channelId))
                channelId = Guid.Empty.ToString();

            var connection = new WebSocketConnection(socket, userId, username);

            ConnectedUsers.AddOrUpdate(
                channelId,
                _ => new List<WebSocketConnection> { connection },
                (_, list) =>
                {
                    list.Add(connection);
                    return list;
                });

            var buffer = new byte[4096];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                if (result.MessageType == WebSocketMessageType.Binary && result.Count > 0)
                {
                    List<WebSocketConnection> connections;
                    if (ConnectedUsers.TryGetValue(channelId, out connections))
                    {
                        foreach (var client in connections/*.Where(c => c.Socket != socket)*/)
                        {
                            await client.Socket.SendAsync(
                                buffer.AsMemory(0, result.Count),
                                WebSocketMessageType.Binary,
                                true,
                                CancellationToken.None
                            );
                        }
                    }
                }
            }
            RemoveConnection(channelId, socket);
        }
        catch (OperationCanceledException)
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Invalid auth", cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AudioWS] Error: {ex.Message}");
        }

        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", cancellationToken);
    }

    private static void RemoveConnection(string channelId, WebSocket socket)
    {
        if (ConnectedUsers.TryGetValue(channelId, out var connections))
        {
            var toRemove = connections.FirstOrDefault(c => c.Socket == socket);
            if (toRemove != null)
                connections.Remove(toRemove);
        }
    }

    public static ConnectedUserDTO[] GetConnectedUsers(string channelId)
    {
        if (string.IsNullOrWhiteSpace(channelId))
            channelId = Guid.Empty.ToString();

        if (!ConnectedUsers.TryGetValue(channelId, out var connections))
            return Array.Empty<ConnectedUserDTO>();

        return connections
            .Select(c => new ConnectedUserDTO(c.UserId, c.Username))
            .ToArray();
    }
}
