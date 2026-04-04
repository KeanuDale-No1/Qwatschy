using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.Services.VoiceService;

namespace VoiceChat.Client.Hubs
{
    public class VoiceHubClient
    {
        private ClientWebSocket ws = new ClientWebSocket();


        private readonly TokenService tokenService;
        private readonly StateService stateService;
        private readonly IVoiceService voiceService;

        public VoiceHubClient(TokenService tokenService,StateService stateService, IVoiceService voiceService)
        {
            this.tokenService = tokenService;
            this.stateService = stateService;
            this.voiceService = voiceService;
            voiceService.AudioFrameReceived += VoiceService_AudioFrameRecorded;
        }
        public async Task ConnectAsync()
        {
            if (string.IsNullOrWhiteSpace(stateService.ServerAddress))
                return;
            if (ws.State == WebSocketState.Open)
                return;
            ws = new ClientWebSocket();
            //ws.Options.SetRequestHeader("Authorization", $"Bearer {tokenService.ReadToken()}");
            var uri = new Uri(stateService.ServerAddress.Replace("http", "ws").Replace("https", "wss") + "/audio?channel=" + stateService.ConnectedChannelId+"&token="+ tokenService.ReadToken());
            await ws.ConnectAsync(uri, CancellationToken.None);
            _ = Task.Run(ReceiveLoop);
        }
        private async void VoiceService_AudioFrameRecorded(byte[] frame)
        {
            if (ws.State == WebSocketState.Open)
            {
                await ws.SendAsync(frame, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
            
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[4096];

            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;


                byte[] opus = new byte[result.Count];
                Buffer.BlockCopy(buffer, 0, opus, 0, result.Count);
                voiceService.PlayOpusChunk(opus);
            }
        }
        public async Task LeaveChannelAsync()
        {
            try
            {
                if (ws.State == WebSocketState.Open)
                {
                    await ws.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Client left channel",
                        CancellationToken.None
                    );
                   
                }
            }
            catch { }

            voiceService.StopRecording();
        }

    }
}
