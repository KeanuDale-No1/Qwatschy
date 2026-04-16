using System;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services.ServerViewService;

namespace VoiceChat.Client.Services.ServerViewServices;

internal class ServerViewService : IServerViewService
{
    public ServerConnectionInfo ServerConnectionInfo { get; private set; }

    public ServerViewService()
    {
        ServerConnectionInfo = new ServerConnectionInfo(Guid.Empty, string.Empty, "Offline");
    }

    public void UpdateServerConnectionInfo(ServerConnectionInfo newInfo)
    {
        ServerConnectionInfo = newInfo;
    }
}