using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Client.Hubs;

namespace VoiceChat.Client.Services.VoiceService
{
    public class VoiceChatService
    {
        private readonly IVoiceService voiceService;
      
        private readonly StateService stateService;
        private bool isRecording = false;
        private bool isInitialized = false;

        public VoiceChatService(IVoiceService voiceService, StateService stateService
            )
        {
            this.stateService = stateService;
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
            stateService.SetMuted(!stateService.IsMuted);
            if (stateService.IsMuted)
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
            if (stateService.ConnectedChannelId.HasValue && stateService.ConnectedChannelId != Guid.Empty)
            {

            }
        }

    }
}
