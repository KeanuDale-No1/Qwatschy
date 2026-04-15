using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services.DialogService;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;

public partial class ServerConnectionsViewModel : ViewModelBase
{
    public ObservableCollection<ServerConnectionInfo> Servers => _clientHub.ServerConnectionInfos;

    private readonly IDialogService _dialogService;
    private readonly ClientHub _clientHub;

    public ServerConnectionsViewModel(IDialogService dialogService, ClientHub clientHub)
    {
        _dialogService = dialogService;
        _clientHub = clientHub;
    }

    [RelayCommand]
    public async Task AddServer()
    {
        var result = await _dialogService.ShowDialog<AddServerDialogViewModel>();
        if (result is null || result.IsCanceled)
            return;

        if (result.Data is string serverAddress)
        {
            await _clientHub.AddServerAsync(serverAddress);
        }
    }

    [RelayCommand]
    public async Task RemoveServer(ServerConnectionInfo server)
    {
        await _clientHub.RemoveServerAsync(server.ServerId);
    }

    [RelayCommand]
    public async Task OpenServer()
    {
    }

    public void Initialize()
    {
        _clientHub.ConnectAllAsync();
    }
}
