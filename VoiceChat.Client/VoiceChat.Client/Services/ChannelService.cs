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


        public async Task LoadChannelInitial()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(stateService.ServerAddress))
                    return;

                var response = await httpClientService.PostAsync<GetChannelsRequestDTO, GetChannelsResponseDTO>("api/GetChannels", new GetChannelsRequestDTO());
                Channels.Clear();
                foreach (var c in response.Channels)
                    Channels.Add(c);
            }
            catch
            {
            }
        }

        internal void SetSelectChannel(ChannelDTO channel)
        {
            var index = Channels.IndexOf(channel);
            if (index >= 0)
            {
                var updated = new ChannelDTO
                {
                    Id = channel.Id,
                    Name = channel.Name,
                    Description = channel.Description,
                    UnreadCount = 0
                };
                Channels[index] = updated;
            }
            
            stateService.SetSelectedChannel(channel.Id);
            ChannelUsers.Clear();
            foreach (var item in Users.Where(u => u.ChannelId == stateService.SelectChannelId))
            {
                ChannelUsers.Add(item);
            }
        }

        public async Task KickUser(Guid channelId, Guid userId)
        {
            await serviceHub.KickUser(channelId, userId);
        }

        public async Task BanUser(Guid channelId, Guid userId)
        {
            await serviceHub.BanUser(channelId, userId);
        }

        public void MarkChannelAsUnread(Guid channelId)
        {
            var channel = Channels.FirstOrDefault(c => c.Id == channelId);
            if (channel != null)
            {
                var index = Channels.IndexOf(channel);
                var updated = new ChannelDTO
                {
                    Id = channel.Id,
                    Name = channel.Name,
                    Description = channel.Description,
                    UnreadCount = channel.UnreadCount + 1
                };
                Channels[index] = updated;
            }
        }

        public void ClearUnread(Guid channelId)
        {
            var channel = Channels.FirstOrDefault(c => c.Id == channelId);
            if (channel != null)
            {
                var index = Channels.IndexOf(channel);
                var updated = new ChannelDTO
                {
                    Id = channel.Id,
                    Name = channel.Name,
                    Description = channel.Description,
                    UnreadCount = 0
                };
                Channels[index] = updated;
            }
        }
    }
}