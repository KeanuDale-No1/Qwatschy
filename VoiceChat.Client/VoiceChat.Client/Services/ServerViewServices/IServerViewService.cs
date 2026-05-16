using System;
using VoiceChat.Client.Models;

namespace VoiceChat.Client.Services.ServerViewService;

public interface IServerViewService
{
    public event Action OpenServerInfoChanged;
    public ServerConnectionInfo? OpenServerInfo { get;  }
    public ChannelInfo? SelectedChannelInfo { get;  }


    public void UpdateServerConnectionInfo(ServerConnectionInfo? newInfo);

    public void UpdateChannelInfo(ChannelInfo? newChannelInfo);

}
