using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VoiceChat.Shared.Networking;

namespace VoiceChat.Desktop.Networking
{
    public class WebSocketService : IWebSocketService, IDisposable
    {
        ClientWebSocket? _socket;
        CancellationTokenSource? _receiveCts;
        public event Action<SocketMessage>? OnMessage;

        public bool IsConnected => _socket?.State == WebSocketState.Open;

        public async Task ConnectAsync(Uri uri, CancellationToken ct = default)
        {
            if (IsConnected) return;

            _socket = new ClientWebSocket();
            await _socket.ConnectAsync(uri, ct);

            _receiveCts = new CancellationTokenSource();
            _ = ReceiveLoop(_receiveCts.Token);
        }

        async Task ReceiveLoop(CancellationToken ct)
        {
            var buffer = new byte[4096];
            using var ms = new MemoryStream();

            try
            {
                while (!ct.IsCancellationRequested && _socket?.State == WebSocketState.Open)
                {
                    var result = await _socket!.ReceiveAsync(buffer, ct);
                    if (result.Count > 0)
                    {
                        ms.Write(buffer, 0, result.Count);
                    }

                    if (result.EndOfMessage)
                    {
                        var json = Encoding.UTF8.GetString(ms.ToArray());
                        ms.SetLength(0);
                        try
                        {
                            var msg = JsonSerializer.Deserialize<SocketMessage>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            if (msg != null) OnMessage?.Invoke(msg);
                        }
                        catch
                        {
                            // ignore malformed messages for now
                        }
                    }

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch { }
        }

        public async Task SendAsync(SocketMessage msg, CancellationToken ct = default)
        {
            if (_socket?.State != WebSocketState.Open) throw new InvalidOperationException("Not connected");
            var json = JsonSerializer.Serialize(msg);
            var bytes = Encoding.UTF8.GetBytes(json);
            var seg = new ArraySegment<byte>(bytes);
            await _socket.SendAsync(seg, WebSocketMessageType.Text, true, ct);
        }

        public async Task DisconnectAsync()
        {
            _receiveCts?.Cancel();
            if (_socket != null && (_socket.State == WebSocketState.Open || _socket.State == WebSocketState.CloseReceived))
            {
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client close", CancellationToken.None);
            }
            _socket?.Dispose();
            _socket = null;
        }

        public void Dispose()
        {
            _receiveCts?.Cancel();
            _socket?.Dispose();
        }
    }
}
