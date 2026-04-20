using System;
using VoiceChat.Client.Models;

namespace VoiceChat.Client.Services.ServerViewService;

public interface IServerViewService
{
    public event Action ServerConnectionInfoChanged;
    public ServerConnectionInfo ServerConnectionInfo { get; }

    public void UpdateServerConnectionInfo(ServerConnectionInfo newInfo);
}
