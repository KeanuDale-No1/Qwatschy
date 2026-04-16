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
        public VoiceChatService(IVoiceService voiceService)
        {
            this.voiceService = voiceService;
            voiceService.AudioFrameReceived += OnAudioFrameReceived;
        }

        private void OnReceiveAudioFrame(byte[] opusdata)
        {
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

        private void OnAudioFrameReceived(byte[] opusdata)
        {

        }

    }
}
