using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using VoiceChat.Shared.Networking;

public class WebSocketHandler
{
    public static async Task Handle(WebSocket socket)
    {
        var buffer = new byte[4096];
        using var ms = new MemoryStream();
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                // Close requested by client
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    try
                    {
                        await socket.CloseAsync(result.CloseStatus ?? WebSocketCloseStatus.NormalClosure, result.CloseStatusDescription, CancellationToken.None);
                    }
                    catch { }
                    break;
                }

                if (result.Count > 0)
                {
                    ms.Write(buffer, 0, result.Count);
                }

                if (result.EndOfMessage)
                {
                    ms.Position = 0;

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string? message;
                        using (var sr = new StreamReader(ms, Encoding.UTF8, false, 1024, leaveOpen: true))
                        {
                            message = await sr.ReadToEndAsync();
                        }

                        ms.SetLength(0);

                        try
                        {
                            var msg = JsonSerializer.Deserialize<SocketMessage>(message, jsonOptions);
                            Console.WriteLine($"Received: {msg?.Type}");

                            var response = new SocketMessage
                            {
                                Type = "server_response",
                                Data = "Hello from server"
                            };

                            var json = JsonSerializer.Serialize(response, jsonOptions);
                            var bytes = Encoding.UTF8.GetBytes(json);
                            await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        catch (JsonException)
                        {
                            // ignore malformed json messages
                        }
                    }
                    else if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        // Binary messages (e.g. audio) can be handled here.
                        // For now we simply discard the data after reading it.
                        ms.SetLength(0);
                    }
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (WebSocketException) { }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket handler error: {ex.Message}");
            try { await socket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Server error", CancellationToken.None); } catch { }
        }
    }
}
