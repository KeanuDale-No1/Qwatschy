using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Client.Hubs;

namespace VoiceChat.Client.Services.VoiceService
{
    public class VoiceChatService
    {
        private readonly IVoiceService voiceService;
      
        private bool isRecording = false;
        private bool isInitialized = false;

        private bool IsMuted = false;

        public event Action<byte[]> AudioFrameReceived;

        public VoiceChatService(IVoiceService voiceService)
        {
            this.voiceService = voiceService;
            voiceService.AudioFrameReceived += VoiceService_AudioFrameReceived; ;
        }

        private void VoiceService_AudioFrameReceived(byte[] obj)
        {
            AudioFrameReceived.Invoke(obj);
        }

        public void Start()
        {
            if (!isInitialized)
            {
                voiceService.InitializeAsync();
                isInitialized = true;
            }
            
            if (!isRecording)
            {
                voiceService.StartRecording();
                isRecording = true;
            }
        }

        public void Stop()
        {
            if (isRecording)
            {
                voiceService.StopRecording();
                isRecording = false;
            }
        }
        public void ToggleMute()
        {
            IsMuted= !IsMuted;
            if (IsMuted)
            {
                voiceService.StopRecording();
                isRecording = false;
            }
            else if (!isRecording)
            {
                voiceService.StartRecording();
                isRecording = true;
            }
        }

        public void PlayOpusChunk(byte[] opusdata)
        {
            voiceService.PlayOpusChunk(opusdata);
        }

    }
}
