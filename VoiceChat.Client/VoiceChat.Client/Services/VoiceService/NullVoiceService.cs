using System;

namespace VoiceChat.Client.Services.VoiceService;

public class NullVoiceService : IVoiceService
{
    public event Action<byte[]>? AudioFrameReceived;

    public void InitializeAsync() { }

    public void StartRecording() { }

    public void StopRecording() { }

    public void PlayOpusChunk(byte[] opusData) { }
}
