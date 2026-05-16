using System;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using VoiceChat.Client.Services.VoiceService;

namespace VoiceChat.Client.Browser.Services;

[SupportedOSPlatform("browser")]
public partial class BrowserVoiceService : IVoiceService
{
    internal static BrowserVoiceService? Instance;

    public event Action<byte[]>? AudioFrameReceived;

    public BrowserVoiceService()
    {
        Instance = this;
    }

    [JSImport("init", "audioService")]
    internal static partial void InitJs();

    [JSImport("startRecording", "audioService")]
    internal static partial void StartRecordingJs();

    [JSImport("stopRecording", "audioService")]
    internal static partial void StopRecordingJs();

    [JSImport("decodeAndPlayPCM", "audioService")]
    internal static partial void DecodeAndPlayPcm(byte[] pcmData);

    [JSImport("playOpusChunk", "audioService")]
    internal static partial void PlayOpusChunkJs(byte[] opusData);

    public void InitializeAsync()
    {
        InitJs();
        Console.WriteLine("[BrowserVoiceService] Initialized");
    }

    public void StartRecording()
    {
        StartRecordingJs();
        Console.WriteLine("[BrowserVoiceService] Recording started");
    }

    public void StopRecording()
    {
        StopRecordingJs();
        Console.WriteLine("[BrowserVoiceService] Recording stopped");
    }

    [JSExport]
    public static void OnOpusFrame(byte[] opusData)
    {
        try
        {
            Instance?.AudioFrameReceived?.Invoke(opusData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BrowserVoiceService.OnOpusFrame] Error: {ex.Message}");
        }
    }

    [JSExport]
    public static void OnPcmFrame(byte[] pcmData)
    {
        try
        {
            Instance?.AudioFrameReceived?.Invoke(pcmData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BrowserVoiceService.OnPcmFrame] Error: {ex.Message}");
        }
    }

    public void PlayOpusChunk(byte[] data)
    {
        if (data == null || data.Length == 0)
            return;
            
        Console.WriteLine($"[BrowserVoiceService] PlayOpusChunk called with {data.Length} bytes");
        PlayOpusChunkJs(data);
    }
}