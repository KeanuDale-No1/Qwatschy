using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.MainArea
{
    public class UserModel { public Guid ClientID { get; set; } public string DisplayName { get; set; } }
    public class Channel {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public class ChannelSidebarViewModel: ViewModelBase
    {
        IHttpClientService httpClientService;
        public ChannelSidebarViewModel(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;

            _ = Task.Run(async () => { var channels = await httpClientService.GetJsonAsync<List<ChannelDTO>>("api/channels"); Channels = new ObservableCollection<Channel>(list: channels?.Select(x => new Channel() { Description = x.Description, Id = x.Id, Name = x.Name }).ToList()); });

        }

        public ChannelSidebarViewModel() : this(null!) { }


        public ObservableCollection<Channel> Channels { get; set; } = new()
    {
        new Channel { Id = Guid.NewGuid(), Name = "General" },
        new Channel { Id = Guid.NewGuid(), Name = "Gaming" },
        new Channel { Id = Guid.NewGuid(), Name = "Music" },
    };

        public ObservableCollection<UserModel> OnlineUsers { get; set; } = new()
    {
        new UserModel { ClientID = Guid.NewGuid(), DisplayName = "Alice" },
        new UserModel { ClientID= Guid.NewGuid(), DisplayName = "Alice2" },
        new UserModel { ClientID= Guid.NewGuid(), DisplayName = "Alice3" },
    };
    }
}