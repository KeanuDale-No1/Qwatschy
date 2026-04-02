using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoiceChat.Client.Services.Audio;
using VoiceChat.Client.Services.VoiceService;
using VoiceChat.Client.Desktop.Services.VoiceService;

namespace VoiceChat.Client.Desktop.Services.Audio;

public class DesktopAudioStreamService : IAudioStreamService, IDisposable
{
    private readonly IVoiceService _voiceService;
    private ClientWebSocket? _webSocket;
    private CancellationTokenSource? _cts;
    private readonly ConcurrentDictionary<Guid, string> _connectedUsers = new();
    
    public event Action<byte[]>? AudioDataReceived;
    public event Action<Guid, string>? UserJoined;
    public event Action<Guid>? UserLeft;
    public event Action<bool>? ConnectionStateChanged;

    public bool IsConnected => _webSocket?.State == WebSocketState.Open;

    public DesktopAudioStreamService(IVoiceService voiceService)
    {
        _voiceService = voiceService;
    }

    public async Task ConnectAsync(string serverUrl, string token, Guid channelId, string username)
    {
        if (_webSocket != null)
        {
            await DisconnectAsync();
        }

        ConnectionStateChanged?.Invoke(false);
        _cts = new CancellationTokenSource();

        _webSocket = new ClientWebSocket();
        _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {token}");

        try
        {
            var uri = new Uri(serverUrl.Replace("http", "ws").Replace("https", "wss"));
            await _webSocket.ConnectAsync(uri, _cts.Token);

            if (_webSocket.State == WebSocketState.Open)
            {
                // Send auth message
                var authMessage = new { token, channelId, username };
                var authJson = System.Text.Json.JsonSerializer.Serialize(authMessage);
                var authBytes = Encoding.UTF8.GetBytes(authJson);
                await _webSocket.SendAsync(
                    new ArraySegment<byte>(authBytes),
                    WebSocketMessageType.Text,
                    true,
                    _cts.Token);

                // Start listening
                _ = ListenAsync(_cts.Token);

                // Initialize voice service for capture/playback
                _voiceService.InitializeAsync();
                _voiceService.AudioFrameReceived += OnLocalAudioFrame;

                ConnectionStateChanged?.Invoke(true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DesktopAudio] Connection error: {ex}");
            ConnectionStateChanged?.Invoke(false);
            throw;
        }
    }

    private void OnLocalAudioFrame(byte[] opusData)
    {
        if (_webSocket?.State == WebSocketState.Open)
        {
            _ = SendAudioFrameAsync(opusData);
        }
    }

    public async Task SendAudioFrameAsync(byte[] audioData)
    {
        if (_webSocket?.State != WebSocketState.Open)
            return;

        try
        {
            await _webSocket.SendAsync(
                new ArraySegment<byte>(audioData),
                WebSocketMessageType.Binary,
                false,
                _cts?.Token ?? CancellationToken.None);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DesktopAudio] Send error: {ex}");
        }
    }

    private async Task ListenAsync(CancellationToken cancellationToken)
    {
        var buffer = new byte[8192];

        try
        {
            while (_webSocket?.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                var result = await _webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Binary && result.Count > 0)
                {
                    var audioData = new byte[result.Count];
                    Buffer.BlockCopy(buffer, 0, audioData, 0, result.Count);
                    
                    // Play audio through voice service
                    _voiceService.PlayOpusChunk(audioData);
                    AudioDataReceived?.Invoke(audioData);
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    HandleTextMessage(json);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Normal closure
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DesktopAudio] Listen error: {ex}");
        }
        finally
        {
            ConnectionStateChanged?.Invoke(false);
        }
    }

    private void HandleTextMessage(string json)
    {
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var type = doc.RootElement.GetProperty("type").GetString();

            if (type == "user_joined")
            {
                var userId = Guid.Parse(doc.RootElement.GetProperty("userId").GetString()!);
                var username = doc.RootElement.GetProperty("username").GetString() ?? "";
                _connectedUsers[userId] = username;
                UserJoined?.Invoke(userId, username);
            }
            else if (type == "user_left")
            {
                var userId = Guid.Parse(doc.RootElement.GetProperty("userId").GetString()!);
                _connectedUsers.TryRemove(userId, out _);
                UserLeft?.Invoke(userId);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DesktopAudio] Parse error: {ex}");
        }
    }

    public async Task DisconnectAsync()
    {
        _voiceService.StopRecording();
        _voiceService.AudioFrameReceived -= OnLocalAudioFrame;

        _cts?.Cancel();

        if (_webSocket != null)
        {
            try
            {
                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Disconnecting",
                        CancellationToken.None);
                }
            }
            catch { }

            _webSocket.Dispose();
            _webSocket = null;
        }

        _cts?.Dispose();
        _cts = null;

        _connectedUsers.Clear();
        ConnectionStateChanged?.Invoke(false);
    }

    public void Dispose()
    {
        DisconnectAsync().GetAwaiter().GetResult();
    }
}
