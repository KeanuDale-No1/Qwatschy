using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            voiceService.AudioFrameReceived += VoiceService_AudioFrameReceived;
        }

        private void VoiceService_AudioFrameReceived(byte[] obj)
        {
            AudioFrameReceived?.Invoke(obj);
        }

        public async Task Start()
        {
            Console.WriteLine("[VoiceChatService] Start called");
            if (!isInitialized)
            {
                Console.WriteLine("[VoiceChatService] Initializing...");
                voiceService.InitializeAsync();
                isInitialized = true;
            }
            await Task.Delay(100);
            if (!isRecording)
            {
                Console.WriteLine("[VoiceChatService] Starting recording...");
                voiceService.StartRecording();
                isRecording = true;
            }
            Console.WriteLine("[VoiceChatService] Start complete");
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
        Console.WriteLine($"[VoiceChatService] PlayOpusChunk called with {opusdata.Length} bytes");
        voiceService.PlayOpusChunk(opusdata);
    }

    }
}
