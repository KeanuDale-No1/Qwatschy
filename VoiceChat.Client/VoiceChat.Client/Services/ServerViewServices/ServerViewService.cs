using CommunityToolkit.Mvvm.ComponentModel;
using System;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Models;
using VoiceChat.Client.Services.ServerViewService;

namespace VoiceChat.Client.Services.ServerViewServices;

internal partial class ServerViewService : IServerViewService
{
    public ServerConnectionInfo? OpenServerInfo { get;internal set; }

    public ChannelInfo? SelectedChannelInfo { get;internal set; }


    public ServerViewService()
    {
    }

    public event Action OpenServerInfoChanged;
    public event Action SelectedChannelInfoChanged;

    public void UpdateServerConnectionInfo(ServerConnectionInfo? newInfo)
    {
        if (OpenServerInfo?.ServerId != newInfo.ServerId) { 
            UpdateChannelInfo(null);
        }
        OpenServerInfo = newInfo;
        OpenServerInfoChanged?.Invoke();
    }

    public void UpdateChannelInfo(ChannelInfo? newChannelInfo)
    {
        SelectedChannelInfo = newChannelInfo;
        SelectedChannelInfoChanged?.Invoke();
    }
}