using CommunityToolkit.Mvvm.ComponentModel;
using System;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Models;
using VoiceChat.Client.Services.ServerViewService;

namespace VoiceChat.Client.Services.ServerViewServices;

internal partial class ServerViewService : IServerViewService
{
    public ServerConnectionInfo? ServerConnectionInfo { get;internal set; }

    public ServerViewService()
    {
    }

    public event Action ServerConnectionInfoChanged;

    public void UpdateServerConnectionInfo(ServerConnectionInfo newInfo)
    {
        ServerConnectionInfo = newInfo;
        ServerConnectionInfoChanged?.Invoke();
    }
}