using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.WebSockets;

public class AudioWebSocketHandler
{
    Dictionary<string, List<WebSocket>> channels = new Dictionary<string, List<WebSocket>>();

    public AudioWebSocketHandler()
    {
    }

    public async Task HandleWebSocketAsync(string? channelId, WebSocket socket, CancellationToken cancellationToken)
    {
        
        try
        {
            if (string.IsNullOrWhiteSpace(channelId))
                channelId = Guid.Empty.ToString();

            if (!channels.ContainsKey(channelId))
                channels[channelId] = new List<WebSocket>();
            channels[channelId].Add(socket);

            var buffer = new byte[4096];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                if (result.MessageType == WebSocketMessageType.Binary && result.Count > 0)
                {
                    foreach (var client in channels[channelId].Where(c => c != socket))
                    {
                        await client.SendAsync(
                            buffer.AsMemory(0, result.Count),
                            WebSocketMessageType.Binary,
                            true,
                            CancellationToken.None
                        );
                    }
                }
            }
            channels[channelId].Remove(socket);
        }
        catch (OperationCanceledException)
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Invalid auth", cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AudioWS] Error: {ex.Message}");
        }
    }
}
