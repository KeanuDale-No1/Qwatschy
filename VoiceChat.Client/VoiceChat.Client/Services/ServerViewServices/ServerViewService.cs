using CommunityToolkit.Mvvm.ComponentModel;
using System;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Models;
using VoiceChat.Client.Services.ServerViewService;

namespace VoiceChat.Client.Services.ServerViewServices;

internal partial class ServerViewService : IServerViewService
{
    public ServerConnectionInfo? OpenServerInfo { get;internal set; }

    public ServerViewService()
    {
    }

    public event Action OpenServerInfoChanged;

    public void UpdateServerConnectionInfo(ServerConnectionInfo newInfo)
    {
        OpenServerInfo = newInfo;
        OpenServerInfoChanged?.Invoke();
    }
}