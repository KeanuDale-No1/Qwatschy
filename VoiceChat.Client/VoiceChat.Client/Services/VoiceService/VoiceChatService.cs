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
        private readonly ConnectionService connectionService;
        private bool isMuted = false;

        public VoiceChatService(IVoiceService voiceService, ConnectionService connectionService, ServiceHubClient serviceHub)
        {
            this.connectionService = connectionService;
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
            isMuted = !isMuted;
            if (isMuted)
            {
                voiceService.StopRecording();
            }
            else {
                voiceService.StartRecording();
            }
        }

        private void OnAudioFrameReceived(byte[] opusdata)
        {
            if (connectionService.ConnectedChannelId.HasValue && connectionService.ConnectedChannelId != Guid.Empty)
            {
                serviceHub.SendAudioFrame(connectionService.ConnectedChannelId.Value, opusdata);
            }
        }

    }
}
