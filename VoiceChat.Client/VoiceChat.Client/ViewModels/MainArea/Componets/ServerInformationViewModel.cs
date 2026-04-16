using CommunityToolkit.Mvvm.ComponentModel;
using System;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services.ServerViewService;
using VoiceChat.Client.Services.ServerViewServices;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;

public partial class ServerInformationViewModel : ViewModelBase
{
    // public string ServerName { get; set; } = "Qwatschy";
    [ObservableProperty] private ServerConnectionInfo serverConnectionInfo;

    private readonly IServerViewService serverViewService;
    public ServerInformationViewModel(IServerViewService serverViewService)
    {
        this.serverViewService = serverViewService;
        serverConnectionInfo = serverViewService.ServerConnectionInfo;
        serverViewService.ServerConnectionInfoChanged += OnServerConnectionInfoChanged;
    }

    private void OnServerConnectionInfoChanged()
    {
        ServerConnectionInfo = serverViewService.ServerConnectionInfo;
    }
}
