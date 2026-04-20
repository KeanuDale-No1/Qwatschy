using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.Services.ServerViewService;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea;

public partial class MainAreaViewModel : ViewModelBase
{
    [ObservableProperty] bool isBusy = false;
    [ObservableProperty] private bool isConnected = false;
    private readonly ClientHub clientHub;
    private readonly IServerViewService serverViewService;

    public MainAreaViewModel(ClientHub clientHub, IServerViewService serverViewService)
    {
        this.clientHub = clientHub;
        this.serverViewService = serverViewService;
        serverViewService.ServerConnectionInfoChanged += OnServerConnectionInfoChanged;
        IsConnected = serverViewService.ServerConnectionInfo != null;
    }

    private void OnServerConnectionInfoChanged()
    {
        IsConnected = serverViewService.ServerConnectionInfo != null;
    }

    public async Task Init()
    {
        IsBusy = true;
        await clientHub.ConnectAllAsync();
        IsBusy = false;
    }
}