using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Client.Hubs;

namespace VoiceChat.Client.Services.VoiceService
{
    public class VoiceChatService
    {
        private readonly IVoiceService voiceService;
        private readonly ServiceHubClient serviceHub;
        private readonly StateService stateService;

        public VoiceChatService(IVoiceService voiceService, StateService stateService, ServiceHubClient serviceHub)
        {
            this.stateService = stateService;
            this.serviceHub = serviceHub;
            this.voiceService = voiceService;
            voiceService.AudioFrameReceived += OnAudioFrameReceived;
            serviceHub.OnReceiveAudioFrame += ServiceHub_OnReceiveAudioFrame;
        }

        private void ServiceHub_OnReceiveAudioFrame(byte[] opusdata)
        {
            voiceService.PlayOpusChunk(opusdata);
        }

        public void Start()
        {
            voiceService.InitializeAsync();
            voiceService.StartRecording();
        }


        public void ToggleMute()
        {
            stateService.SetMuted(!stateService.IsMuted);
            if (stateService.IsMuted)
            {
                voiceService.StopRecording();
            }
            else {
                voiceService.StartRecording();
            }
        }

        private void OnAudioFrameReceived(byte[] opusdata)
        {
            if (stateService.ConnectedChannelId.HasValue && stateService.ConnectedChannelId != Guid.Empty)
            {
                serviceHub.SendAudioFrame(stateService.ConnectedChannelId.Value, opusdata);
            }
        }

    }
}
