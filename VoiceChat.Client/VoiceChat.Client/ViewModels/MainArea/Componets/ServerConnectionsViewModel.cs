using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services.DialogService;
using VoiceChat.Client.Services.ServerService;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;



public partial class ServerConnectionsViewModel : ViewModelBase
{
    public ObservableCollection<ServerConnectionInfo> Servers { get; } = new ObservableCollection<ServerConnectionInfo>();
    private readonly IDialogService dialogService;
    private readonly IServerService serverService;
    public ServerConnectionsViewModel(IDialogService dialogService, IServerService serverService, ClientHub clientHub)
    {
        Servers = clientHub.ServerConnectionInfos;
        this.dialogService = dialogService;
        this.serverService = serverService;
    }

    [RelayCommand]
    public async Task AddServer()
    {
        var result = await dialogService.ShowDialog<AddServerDialogViewModel>();
        if (result is null || result.IsCanceled)
            return;

        if (result.Data is string serveradress)
        {
            var success = await serverService.AddServer(serveradress);
        }
    }



    [RelayCommand]
    public async Task RemoveServer(ServerConnectionInfo server)
    {
    }

    [RelayCommand]
    public async Task OpenServer()
    {
    }

    public void Initialize()
    {
        serverService.ConnectAll();
    }

}


public static class AbbreviationHelper
{
    public static string GetAbbreviation(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return new string(input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length > 2)
            .SelectMany(w => w.Where(char.IsUpper))
            .Take(3)
            .ToArray());
    }
}

public class ServerConnectionInfo
{
    public ServerConnectionInfo(Guid serverId, string serverAdress, string serverName)
    {
        this.ServerId = serverId;
        this.ServerAdress = serverAdress;
        this.ServerName = serverName;
    }

    public Guid ServerId { get; set; }
    public string ServerAdress { get; set; }
    public string ServerName { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsConnected { get; set; }
    public string Abbr => AbbreviationHelper.GetAbbreviation(ServerName);
}


