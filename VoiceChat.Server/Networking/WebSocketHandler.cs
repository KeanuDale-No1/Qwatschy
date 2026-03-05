using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using VoiceChat.Shared.Networking;

public class WebSocketHandler
{
    public static async Task Handle(WebSocket socket)
    {
        var buffer = new byte[4096];

        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            var msg = JsonSerializer.Deserialize<SocketMessage>(message);

            Console.WriteLine($"Received: {msg?.Type}");

            var response = new SocketMessage
            {
                Type = "server_response",
                Data = "Hello from server"
            };

            var json = JsonSerializer.Serialize(response);

            var bytes = Encoding.UTF8.GetBytes(json);

            await socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}