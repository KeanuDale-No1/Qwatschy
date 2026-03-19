using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services.VoiceService;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Services
{

    public class NotifyingCollection<T> : ObservableCollection<T>
    {
        public void RaiseCollectionChanged()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Reset));
        }
    }

    public class ChannelService
    {
        private readonly AppState appState;
        private readonly StateService stateService;

        private readonly ApiService httpClientService;

        private readonly ServiceHubClient serviceHub;
        private readonly VoiceChatService voiceService;
        public ObservableCollection<ChannelDTO> Channels = new ObservableCollection<ChannelDTO> { };

        public List<UserDTO> Users = new List<UserDTO>();
        public NotifyingCollection<UserDTO> ChannelUsers = new NotifyingCollection<UserDTO> { };

        public ChannelService(ApiService httpClientService,
                              ServiceHubClient serviceHub,
                              VoiceChatService voiceChatService,
                              AppState appState,
                              StateService stateService)
        {
            this.appState = appState;
            this.httpClientService = httpClientService;
            this.serviceHub = serviceHub;
            this.voiceService = voiceChatService;
            this.stateService = stateService;
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
            if (obj.UserId == appState.GetUser().ClientId)
            {
                stateService.SetConnectedChannel(obj.ChannelId);
                voiceService.Start();
            }
            var user = Users.SingleOrDefault(x => x.ClientID == obj.UserId);
            if (user != null)
            {
                user.ChannelId = obj.ChannelId;
            }
            else
                Users.Add(new UserDTO() { ClientID = obj.UserId, DisplayName = obj.Username, ChannelId = obj.ChannelId });

            foreach (var item in Users.Where(u => u.ChannelId == stateService.SelectChannelId))
            {
                if (ChannelUsers.Select(x => x.ClientID).Contains(item.ClientID))
                    continue;
                ChannelUsers.Add(item);
            }
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
            var response = await httpClientService.PostAsync<GetChannelsRequestDTO, GetChannelsResponseDTO>("api/GetChannels", new GetChannelsRequestDTO());
            Channels.Clear();
            foreach (var c in response.Channels)
                Channels.Add(c);
        }

        internal void SetSelectChannel(ChannelDTO channel)
        {
            stateService.SetSelectedChannel(channel.Id);
            ChannelUsers.Clear();
            foreach (var item in Users.Where(u => u.ChannelId == stateService.SelectChannelId))
            {
                ChannelUsers.Add(item);
            }
        }
    }
}