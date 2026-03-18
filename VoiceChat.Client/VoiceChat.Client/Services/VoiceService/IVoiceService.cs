using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services.VoiceService
{
    public interface IVoiceService
    {

        void InitializeAsync();
        void StopRecording();
        void StartRecording();
        event Action<byte[]> AudioFrameReceived;





        void PlayOpusChunk(byte[] pcmData);

    }
}
