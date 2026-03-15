using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public ObservableCollection<ChannelDTO> Channels = new ObservableCollection<ChannelDTO> { };
        public ObservableCollection<UserDTO> Users = new ObservableCollection<UserDTO> { };
        public ChannelService(StatusService statusService, IHttpClientService httpClientService, ServiceHub serviceHub)
        {
            this.statusService = statusService;
            this.httpClientService = httpClientService;
            this.serviceHub = serviceHub;
            LoadChannelInitial();
            serviceHub.ChannelAdd += ServiceHub_ChannelAdd;
            serviceHub.UserJoinChannel += ServiceHub_UserJoinChannel;
            serviceHub.ChannelRemove += ServiceHub_ChannelRemove;
        }

        private void ServiceHub_ChannelRemove(Guid obj)
        {
            var channel = Channels.FirstOrDefault(c => c.Id == obj);
            if (channel != null)
            {
                Channels.Remove(channel);
            }
        }

        private void ServiceHub_UserJoinChannel(ConnectChannelResponseDTO obj)
        {
            var user = Users.SingleOrDefault(x => x.ClientID == obj.UserId);
            if (user != null)
                Users.Remove(user);
            Users.Add(new UserDTO() { ClientID = obj.UserId, DisplayName = obj.Username, ChannelId = obj.ChannelId  });

        }

        private void ServiceHub_ChannelAdd(ChannelDTO obj)
        {
            Channels.Add(obj);
        }

        public async Task AddChannel(ChannelDTO channelDTO)
        {
            await serviceHub.AddChannel(channelDTO);
        }
        public async Task DeleteChannel(ChannelDTO channel)
        {
            await serviceHub.DeleteChannel(channel.Id);
        }
        public async Task JoinChannel(ChannelDTO channel)
        {
            await serviceHub.JoinChannel(channel.Id);
        }


        public async void LoadChannelInitial()
        {
            try
            {
                statusService.AddReport("Lade Kanäle...");
                var response = await httpClientService.PostAsync<GetChannelsRequestDTO, GetChannelsResponseDTO>("api/GetChannels", new GetChannelsRequestDTO());
                Channels.Clear();
                foreach (var c in response.Channels)
                    Channels.Add(c);
                statusService.AddReport("Kanäle geladen");
            }
            catch (Exception ex)
            {
                statusService.AddReport($"Fehler beim laden der Channels {ex.Message}");
            }
        }

    }
}