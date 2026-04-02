using System;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using VoiceChat.Client.Services.VoiceService;

namespace VoiceChat.Client.Browser.Services;

[SupportedOSPlatform("browser")]
public partial class BrowserVoiceService : IVoiceService
{
    private readonly OpusCodec codec = new OpusCodec();
    internal static BrowserVoiceService? Instance;

    public event Action<byte[]>? AudioFrameReceived;

    public BrowserVoiceService()
    {
        Instance = this;
    }

    public void InitializeAsync()
    {
        // JS-Modul wird in Program.cs importiert
    }

    public void StartRecording()
    {
        StartRecordingJs();
    }

    public void StopRecording()
    {
        StopRecordingJs();
    }

    // JS-Funktionen importieren
    [JSImport("startRecording", "audioService")]
    internal static partial void StartRecordingJs();

    [JSImport("stopRecording", "audioService")]
    internal static partial void StopRecordingJs();

    // Wird von JS aufgerufen
    [JSExport]
    public static void OnPcmFrame(byte[] pcmBytes)
    {
        try
        {

            Instance?.HandlePcmFrame(pcmBytes);

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }
    }

    private void HandlePcmFrame(byte[] pcmBytes)
    {
        short[] pcm = new short[960];
        Buffer.BlockCopy(pcmBytes, 0, pcm, 0, 1920);

        var opus = codec.Encode(pcm);
        AudioFrameReceived?.Invoke(opus);
    }

    public void PlayOpusChunk(byte[] opus)
    {
        // später: JS Playback
    }
}
