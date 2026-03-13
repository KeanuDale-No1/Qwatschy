using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.Login;


public partial class LoginViewModel : ViewModelBase
{
    private readonly StatusService statusService;
    private readonly AppState appState;
    private readonly ConnectionService connectionService;

    [ObservableProperty] public string username = "";
    [ObservableProperty] public string inputserverAddress = "";
    [ObservableProperty] public bool openLastConnection = false;
    [ObservableProperty] public string historyButtonText = ">";

    public ObservableCollection<ServerConnection> ServerConnections { get; } = new ObservableCollection<ServerConnection>();



    public LoginViewModel(AppState appState, ConnectionService connectionService, StatusService statusService)
    {
        this.appState = appState;
        this.statusService = statusService;
        this.connectionService = connectionService;
        Username = appState.GetUser().UserName;
        InputserverAddress = appState.GetLastServer()?.ServerAdress ?? "";
        ServerConnections = new ObservableCollection<ServerConnection>(appState.GetLastServers());
    }

    [RelayCommand]
    public async Task Conntect()
    {
        try
        {
            await connectionService.ServerConnect(Username, InputserverAddress);
        }
        catch (Exception ex)
        {
            statusService.AddReport($"Error {ex}");
        }
    }

    [RelayCommand]
    public async Task OpenHistory()
    {
        OpenLastConnection = !OpenLastConnection;
        HistoryButtonText = OpenLastConnection ? "<" : ">";
    }
    
    [RelayCommand]
    public async Task SetHistoryServer(ServerConnection serveradress)
    {
        InputserverAddress = serveradress.ServerAdress;
        Username = serveradress.UserName;
    }


}
