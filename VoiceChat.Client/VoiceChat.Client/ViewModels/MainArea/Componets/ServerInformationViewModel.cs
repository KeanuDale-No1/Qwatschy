using CommunityToolkit.Mvvm.ComponentModel;
using System;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Models;
using VoiceChat.Client.Services.ServerViewService;
using VoiceChat.Client.Services.ServerViewServices;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;

public partial class ServerInformationViewModel : ViewModelBase
{
    [ObservableProperty] private ServerConnectionInfo serverConnectionInfo;

    private readonly IServerViewService serverViewService;
    public ServerInformationViewModel(IServerViewService serverViewService)
    {
        this.serverViewService = serverViewService;
        serverConnectionInfo = serverViewService.OpenServerInfo;
        serverViewService.OpenServerInfoChanged += OnServerConnectionInfoChanged;
    }

    private void OnServerConnectionInfoChanged()
    {
        ServerConnectionInfo = serverViewService.OpenServerInfo;
    }
}
