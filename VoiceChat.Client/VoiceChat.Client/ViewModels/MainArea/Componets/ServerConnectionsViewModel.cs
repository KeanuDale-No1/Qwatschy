using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;



public partial class ServerConnectionsViewModel : ViewModelBase
{
    public ObservableCollection<ServerConnectionInfo> Servers { get; } = new ObservableCollection<ServerConnectionInfo>();

    public ServerConnectionsViewModel()
    {
        
    }

    [RelayCommand]
    public async Task AddServer(ServerConnectionInfo server)
    {
        
    }

    [RelayCommand]
    public async Task RemoveServer(ServerConnectionInfo server)
    {
        
    }

    [RelayCommand]
    public async Task ConnectServer()
    {

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

public record ServerConnectionInfo(string ServerAdress, string ServerName)
{
    public string Abbr => AbbreviationHelper.GetAbbreviation(ServerName);
}


