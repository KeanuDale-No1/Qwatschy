using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using ManagedBass;
using VoiceChat.Client.Services.VoiceService;
using RNNoise.NET; // aus RNNoise.Net o.ä.

namespace VoiceChat.Client.Desktop.Services.VoiceService
{
    public class DesktopVoiceService : IVoiceService, IDisposable
    {
        private int playStream;
        private RecordProcedure? recordProcedure;
        private int recordHandle;
        private readonly OpusCodec codec = new OpusCodec();
        private readonly object recordingLock = new object();

        // RNNoise für Rauschunterdrückung
        private Denoiser rnnoise = new Denoiser();

        // Jitter-Buffer
        private readonly ConcurrentQueue<byte[]> jitterBuffer = new ConcurrentQueue<byte[]>();
        private readonly AutoResetEvent jitterEvent = new AutoResetEvent(false);
        private Thread? playbackThread;
        private bool isRunning;

        public event Action<byte[]>? AudioFrameReceived;

        public void InitializeAsync()
        {
            // Ultra-Low-Latency Bass
            Bass.Configure(Configuration.PlaybackBufferLength, 50); // 50 ms Output-Buffer
            Bass.Configure(Configuration.UpdatePeriod, 5);          // 5 ms Update

            if (!Bass.Init())
                throw new Exception("Bass.Init failed");

            if (!Bass.RecordInit())
                throw new Exception("Bass.RecordInit failed");

            playStream = Bass.CreateStream(48000, 1, BassFlags.Default, StreamProcedureType.Push);
            if (playStream == 0)
                throw new Exception("CreateStream failed");

            isRunning = true;
            playbackThread = new Thread(PlaybackLoop)
            {
                IsBackground = true,
                Name = "VoicePlaybackLoop"
            };
            playbackThread.Start();
        }

        public void StartRecording()
        {
            lock (recordingLock)
            {
                StopRecordingInternal();

                recordProcedure = (handle, buffer, length, user) =>
                {
                    if (length <= 0)
                        return true;

                    // Wir schneiden direkt 20ms-Frames (1920 Bytes) aus dem Bass-Block
                    int offset = 0;
                    while (length - offset >= 1920)
                    {
                        byte[] frame = new byte[1920];
                        Marshal.Copy(buffer + offset, frame, 0, 1920);
                        offset += 1920;

                        // 1920 Bytes → 960 Samples (16 Bit, Mono)
                        short[] pcmShort = new short[960];
                        Buffer.BlockCopy(frame, 0, pcmShort, 0, 1920);

                        // RNNoise erwartet float-Samples
                        float[] floatFrame = new float[960];
                        for (int i = 0; i < 960; i++)
                            floatFrame[i] = pcmShort[i] / 32768f;

                        //High‑Pass Filter (entfernt Brummen & Lüfter)
                        float hpAlpha = 0.995f;
                        float prevInput = 0f;
                        float prevOutput = 0f;

                        for (int i = 0; i < floatFrame.Length; i++)
                        {
                            float input = floatFrame[i];
                            float output = hpAlpha * (prevOutput + input - prevInput);
                            prevInput = input;
                            prevOutput = output;
                            floatFrame[i] = output;
                        }

                        //Rauschunterdrückung
                        rnnoise.Denoise(floatFrame);

                        //Soft Noise Gate
                        float rms = 0f;
                        for (int i = 0; i < floatFrame.Length; i++)
                            rms += floatFrame[i] * floatFrame[i];

                        rms = MathF.Sqrt(rms / floatFrame.Length);

                        if (rms < 0.015f) // -36 dB
                        {
                            for (int i = 0; i < floatFrame.Length; i++)
                                floatFrame[i] *= 0.3f; // nur leiser, nicht abschneiden
                        }

                        // zurück zu PCM
                        for (int i = 0; i < 960; i++)
                        {
                            float v = floatFrame[i];
                            if (v > 1f) v = 1f;
                            if (v < -1f) v = -1f;
                            pcmShort[i] = (short)(v * 32767f);
                        }

                        try
                        {
                            byte[] opusData = codec.Encode(pcmShort);
                            AudioFrameReceived?.Invoke(opusData);
                        }
                        catch
                        {
                            // Encoding-Fehler → Frame droppen
                        }
                    }

                    return true;
                };

                recordHandle = Bass.RecordStart(48000, 1, BassFlags.Default, recordProcedure);
            }
        }

        public void StopRecording()
        {
            lock (recordingLock)
            {
                StopRecordingInternal();
            }
        }

        private void StopRecordingInternal()
        {
            try
            {
                if (recordHandle != 0)
                {
                    Bass.ChannelStop(recordHandle);
                    recordHandle = 0;
                }
            }
            catch { }

            recordProcedure = null;
        }

        public void PlayOpusChunk(byte[] opusdata)
        {
            if (playStream == 0 || opusdata == null || opusdata.Length == 0)
                return;

            // erst in den Jitter-Buffer, nicht direkt abspielen
            jitterBuffer.Enqueue(opusdata);
            jitterEvent.Set();
        }

        private void PlaybackLoop()
        {
            const int targetMs = 60; // Ziel-Latenz im Output-Buffer

            while (isRunning)
            {
                if (!jitterBuffer.TryDequeue(out var opus))
                {
                    jitterEvent.WaitOne(10);
                    continue;
                }

                // Dekodieren
                var pcmShort = codec.Decode(opus);
                var pcmBytes = new byte[pcmShort.Length * 2];
                Buffer.BlockCopy(pcmShort, 0, pcmBytes, 0, pcmBytes.Length);

                Bass.StreamPutData(playStream, pcmBytes, pcmBytes.Length);

                long buffered = Bass.ChannelGetData(playStream, IntPtr.Zero, (int)DataFlags.Available);
                double msAvailable = buffered / (48000.0 * 2.0) * 1000.0;

                if (Bass.ChannelIsActive(playStream) != PlaybackState.Playing && msAvailable >= 10)
                {
                    Bass.ChannelPlay(playStream);
                }

                // Wenn zu viel Puffer → Frames verwerfen, um Latenz zu begrenzen
                if (msAvailable > targetMs + 40)
                {
                    while (msAvailable > targetMs && jitterBuffer.TryDequeue(out _))
                    {
                        buffered = Bass.ChannelGetData(playStream, IntPtr.Zero, (int)DataFlags.Available);
                        msAvailable = buffered / (48000.0 * 2.0) * 1000.0;
                    }
                }
            }
        }

        public void Dispose()
        {
            isRunning = false;
            jitterEvent.Set();
            playbackThread?.Join(200);

            if (playStream != 0)
            {
                Bass.ChannelStop(playStream);
                Bass.StreamFree(playStream);
                playStream = 0;
            }

            if (recordHandle != 0)
            {
                Bass.ChannelStop(recordHandle);
                recordHandle = 0;
            }

            Bass.RecordFree();
            Bass.Free();
        }
    }
}

















//using ManagedBass;
//using SkiaSharp;
//using System;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using VoiceChat.Client.Services.VoiceService;

//namespace VoiceChat.Client.Desktop.Services.VoiceService
//{
//    public class DesktopVoiceService : IVoiceService
//    {
//        private int playStream;
//        private RecordProcedure? recordProcedure;
//        private int recordHandle;
//        private OpusCodec codec = new OpusCodec();
//        private readonly object recordingLock = new object();

//        public event Action<byte[]>? AudioFrameReceived;
//        public void InitializeAsync()
//        {
//            Bass.Init();
//            Bass.Configure(Configuration.PlaybackBufferLength, 50); // 50 ms
//            Bass.Configure(Configuration.UpdatePeriod, 5);          // 5 ms

//            Bass.RecordInit();
//            playStream = Bass.CreateStream(48000, 1, BassFlags.Default, StreamProcedureType.Push);
//            Bass.ChannelPlay(playStream);
//        }


//        bool isPreBuffering = true;
//        public void PlayOpusChunk(byte[] opusdata)
//        {
//            if (playStream == 0) return;

//            var pcmShort = codec.Decode(opusdata);
//            byte[] pcmBytes = new byte[pcmShort.Length * 2];
//            Buffer.BlockCopy(pcmShort, 0, pcmBytes, 0, pcmBytes.Length);

//            Bass.StreamPutData(playStream, pcmBytes, pcmBytes.Length);

//            long buffered = Bass.ChannelGetData(playStream, IntPtr.Zero, (int)DataFlags.Available);
//            double msAvailable = buffered / (48000.0 * 2) * 1000;

//            if (msAvailable > 60)
//            {
//                Bass.ChannelPlay(playStream);
//                isPreBuffering = false;
//            }
//            else if (msAvailable < 10)
//            {
//                isPreBuffering = true;
//            }
//        }

//        public void StartRecording()
//        {
//            lock (recordingLock)
//            {
//                StopRecordingInternal();

//                var recordingBuffer = new List<byte>();
//                recordProcedure = (handle, buffer, length, user) =>
//                {
//                    if (length > 0)
//                    {
//                        byte[] temp = new byte[length];
//                        Marshal.Copy(buffer, temp, 0, length);
//                        recordingBuffer.AddRange(temp);
//                        while (recordingBuffer.Count >= 1920)
//                        {
//                            byte[] frame = recordingBuffer.GetRange(0, 1920).ToArray();
//                            recordingBuffer.RemoveRange(0, 1920);

//                            if (frame.Length < 1920) continue;

//                            short[] pcmShort = new short[960];
//                            Buffer.BlockCopy(frame, 0, pcmShort, 0, 1920);

//                            try
//                            {
//                                byte[] opusData = codec.Encode(pcmShort);
//                                AudioFrameReceived?.Invoke(opusData);
//                            }
//                            catch
//                            {
//                            }
//                        }
//                    }
//                    return true;
//                };
//                recordHandle = Bass.RecordStart(48000, 1, BassFlags.Default, recordProcedure);
//            }
//        }

//        public void StopRecording()
//        {
//            lock (recordingLock)
//            {
//                StopRecordingInternal();
//            }
//        }

//        private void StopRecordingInternal()
//        {
//            try
//            {
//                if (recordHandle != 0)
//                {
//                    Bass.ChannelStop(recordHandle);
//                    recordHandle = 0;
//                }
//            }
//            catch
//            {
//            }
//            recordProcedure = null;
//        }
//    }
//}
