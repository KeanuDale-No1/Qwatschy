using ManagedBass;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Client.Services.VoiceService;

namespace VoiceChat.Client.Desktop.Services.VoiceService
{
    public class DesktopVoiceService : IVoiceService
    {
        private int playStream;
        private RecordProcedure? recordProcedure;
        private int recordHandle;
        private OpusCodec codec = new OpusCodec();
        private readonly object recordingLock = new object();

        public event Action<byte[]>? AudioFrameReceived;
        public void InitializeAsync()
        {
            Bass.Init();
            Bass.RecordInit();
            playStream = Bass.CreateStream(48000, 1, BassFlags.Default,StreamProcedureType.Push);
            Bass.ChannelPlay(playStream);
        }


        bool isPreBuffering = true;
        public void PlayOpusChunk(byte[] opusdata)
        {
            if (playStream == 0) return;
            
            var pcmShort = codec.Decode(opusdata);
            byte[] pcmBytes = new byte[pcmShort.Length *2] ;
            Buffer.BlockCopy(pcmShort, 0, pcmBytes, 0, pcmBytes.Length);

            Bass.StreamPutData(playStream, pcmBytes, pcmBytes.Length);

            long buffered = Bass.ChannelGetData(playStream, IntPtr.Zero, (int)DataFlags.Available);
            double msAvailable = buffered / (48000.0 * 2) * 1000;
            if (isPreBuffering)
            {
                if (msAvailable > 60)
                {
                    Bass.ChannelPlay(playStream);
                    isPreBuffering = false;
                }
            }
            else if (msAvailable < 10) 
            {
                isPreBuffering = true;
            }
        }

        public void StartRecording()
        {
            lock (recordingLock)
            {
                StopRecordingInternal();
                
                var recordingBuffer = new List<byte>();
                recordProcedure = (handle, buffer, length, user) =>
                {
                    if (length > 0)
                    {
                        byte[] temp = new byte[length];
                        Marshal.Copy(buffer, temp, 0, length);
                        recordingBuffer.AddRange(temp);
                        while (recordingBuffer.Count >= 1920)
                        {
                            byte[] frame = recordingBuffer.GetRange(0, 1920).ToArray();
                            recordingBuffer.RemoveRange(0, 1920);

                            if (frame.Length < 1920) continue;

                            short[] pcmShort = new short[960];
                            Buffer.BlockCopy(frame, 0, pcmShort, 0, 1920);

                            try
                            {
                                byte[] opusData = codec.Encode(pcmShort);
                                AudioFrameReceived?.Invoke(opusData);
                            }
                            catch
                            {
                            }
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
            catch
            {
            }
            recordProcedure = null;
        }
    }
}
