using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using VoiceChat.Client.Models;
using VoiceChat.Client.Services.VoiceService;

namespace VoiceChat.Client.Hubs
{
public class VoiceHubClient
    {
        private ClientWebSocket ws = new ClientWebSocket();

        private readonly VoiceChatService voiceService;

        public VoiceHubClient(VoiceChatService voiceService)
        {
            this.voiceService = voiceService;
            voiceService.AudioFrameReceived += VoiceService_AudioFrameRecorded;
        }

        public async Task ConnectAsync(ChannelInfo channelInfo, ServerConnectionInfo connectionInfo)
        {
            if (ws.State == WebSocketState.Open)
                return;

            await LeaveChannelAsync();

            ws = new ClientWebSocket();

            var baseUrl = connectionInfo.ServerAdress
                .Replace("http://", "ws://")
                .Replace("https://", "wss://")
                .TrimEnd('/');

            var url = $"{baseUrl}/audio?channelId={channelInfo.Id}&token={connectionInfo.Token}";
            var uri = new Uri(url);

            await ws.ConnectAsync(uri, CancellationToken.None);
            voiceService.Start();
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
                {
                    Console.WriteLine("[VoiceHubClient] WebSocket closed by server.");
                    break;
                }


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

            voiceService.Stop();
        }

    }
}
