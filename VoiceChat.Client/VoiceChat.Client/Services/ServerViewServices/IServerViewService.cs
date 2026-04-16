using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Client.Hubs;

namespace VoiceChat.Client.Services.ServerViewService;

internal interface IServerViewService
{
    public ServerConnectionInfo ServerConnectionInfo { get; }
    public void UpdateServerConnectionInfo(ServerConnectionInfo newInfo);
}
