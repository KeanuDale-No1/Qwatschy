using System;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services.Audio;

public interface IAudioStreamService
{
    event Action<byte[]>? AudioDataReceived;
    event Action<Guid, string>? UserJoined;
    event Action<Guid>? UserLeft;
    event Action<bool>? ConnectionStateChanged;

    bool IsConnected { get; }
    
    Task ConnectAsync(string serverUrl, string token, Guid channelId, string username);
    Task DisconnectAsync();
    Task SendAudioFrameAsync(byte[] audioData);
}
