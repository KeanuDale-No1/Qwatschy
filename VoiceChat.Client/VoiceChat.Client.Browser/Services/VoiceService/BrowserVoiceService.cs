using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Threading.Channels;
using System.Threading.Tasks;
using VoiceChat.Client.Services.VoiceService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VoiceChat.Client.Browser.Services;

[SupportedOSPlatform("browser")]
public partial class BrowserVoiceService : IVoiceService
{
    private readonly OpusCodec codec = new OpusCodec();
    private readonly short[] _pcmBuffer = new short[960];
    internal static BrowserVoiceService? Instance;

    public event Action<byte[]>? AudioFrameReceived;
    private readonly Channel<byte[]> _encodeQueue = Channel.CreateUnbounded<byte[]>();
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
    internal static partial void DecodeAndPlay(byte[] chunk);

    public void InitializeAsync()
    {
        InitJs();
    }

    public void StartRecording()
    {
        StartRecordingJs();
    }

    public void StopRecording()
    {
        StopRecordingJs();
    }


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

        int totalSamples = pcmBytes.Length / 2;
        short[] pcmShort = new short[totalSamples];
        Buffer.BlockCopy(pcmBytes, 0, pcmShort, 0, pcmBytes.Length);

        int offset = 0;
        while (offset + 960 <= totalSamples)
        {
            short[] frame960 = new short[960];
            Array.Copy(pcmShort, offset, frame960, 0, 960);

            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                byte[] opusData = codec.Encode(frame960);
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
                AudioFrameReceived?.Invoke(opusData);
            }
            catch
            {
                // Frame droppen bei Fehler
            }

            offset += 960;
        }
    }


    public void PlayOpusChunk(byte[] data)
    {
        DecodeAndPlay(data);
    }

   

}
