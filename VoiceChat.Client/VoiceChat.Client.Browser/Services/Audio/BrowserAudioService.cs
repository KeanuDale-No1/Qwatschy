using System;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using VoiceChat.Client.Services.Audio;

namespace VoiceChat.Client.Browser.Services.Audio;

[SupportedOSPlatform("browser")]
public partial class BrowserAudioService : IAudioStreamService
{
    public event Action<byte[]>? AudioDataReceived;
    public event Action<Guid, string>? UserJoined;
    public event Action<Guid>? UserLeft;
    public event Action<bool>? ConnectionStateChanged;

    public bool IsConnected => _isConnected;
    private bool _isConnected;

    [JSImport("initAudio", "AudioService")]
    public static partial Task InitAudioAsync();

    [JSImport("connect", "AudioService")]
    public static partial Task ConnectJsAsync(string url, string token, string channelId, string username);

    [JSImport("disconnect", "AudioService")]
    public static partial Task DisconnectJsAsync();

    [JSImport("isConnected", "AudioService")]
    public static partial Task<bool> IsConnectedAsync();

    public async Task ConnectAsync(string serverUrl, string token, Guid channelId, string username)
    {
        Console.WriteLine($"[BrowserAudio] ConnectAsync called with URL: {serverUrl}");
        
        try
        {
            ConnectionStateChanged?.Invoke(false);
            
            Console.WriteLine("[BrowserAudio] Initializing audio...");
            await InitAudioAsync();
            
            Console.WriteLine($"[BrowserAudio] Connecting to WebSocket with channelId: {channelId}");
            await ConnectJsAsync(serverUrl, token, channelId.ToString(), username);
            
            _isConnected = true;
            ConnectionStateChanged?.Invoke(true);
            Console.WriteLine("[BrowserAudio] Connected successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BrowserAudio] Connection error: {ex}");
            _isConnected = false;
            ConnectionStateChanged?.Invoke(false);
            throw;
        }
    }

    public async Task DisconnectAsync()
    {
        try
        {
            await DisconnectJsAsync();
        }
        catch { }
        
        _isConnected = false;
        ConnectionStateChanged?.Invoke(false);
    }

    public Task SendAudioFrameAsync(byte[] audioData)
    {
        return Task.CompletedTask;
    }
}