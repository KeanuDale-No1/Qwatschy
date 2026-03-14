using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Services
{
    public class ChannelService
    {
        private readonly StatusService statusService;

        private readonly IHttpClientService httpClientService;

        private readonly ServiceHub serviceHub;

        public ChannelDTO[] Channels = new ChannelDTO[] { };

        public ChannelService(StatusService statusService, IHttpClientService httpClientService, ServiceHub serviceHub)
        {
            this.statusService = statusService;
            this.httpClientService = httpClientService;
            this.serviceHub = serviceHub;
            LoadChannels();
            
        }

        public async Task AddChannel(ChannelDTO channelDTO)
        {
            await serviceHub.AddChannel(channelDTO);
        }

        public async Task JoinChannel(ChannelDTO channel)
        {

        }


        public async void LoadChannels()
        {
            try
            {
                statusService.AddReport("Lade Kanäle...");
                var response = await httpClientService.PostAsync<GetChannelsRequestDTO, GetChannelsResponseDTO>("api/GetChannels", new GetChannelsRequestDTO());
                Channels = response.Channels;
                statusService.AddReport("Kanäle geladen");

            }
            catch (Exception ex)
            {
                statusService.AddReport($"Fehler beim laden der Channels {ex.Message}");
            }
        }




    }
}