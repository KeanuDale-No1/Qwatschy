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
    private readonly AppState appState;
    private readonly ConnectionService connectionService;

    [ObservableProperty] public string username = "";
    [ObservableProperty] public string inputserverAddress = "";
    [NotifyPropertyChangedFor(nameof(HistoryButtonText))]
    [ObservableProperty] public bool openLastConnection = false;
    public string HistoryButtonText =>OpenLastConnection? "Verstecke Historie" : "Zeige Historie";

    public ObservableCollection<ServerConnection> ServerConnections { get; } = new ObservableCollection<ServerConnection>();



    public LoginViewModel(AppState appState, ConnectionService connectionService)
    {
        if (appState == null)
            return;

        this.appState = appState;
        this.connectionService = connectionService;
        Username = appState.GetUser().UserName;
        InputserverAddress = appState.GetLastServer()?.ServerAdress ?? "";
        ServerConnections = new ObservableCollection<ServerConnection>(appState.GetLastServers());
    }
    public LoginViewModel() :this(null!,null!){ }

    [RelayCommand]
    public async Task Conntect()
    {
        if (string.IsNullOrWhiteSpace(InputserverAddress))
        {
            Console.WriteLine("serveradresse nicht eingegeben");
            Console.WriteLine($"serveradresse: {InputserverAddress}");
            return;
        }
            
        appState.SetUsername(Username);
        try
        {
            await connectionService.ServerConnect(InputserverAddress);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler bei {nameof(LoginViewModel).ToString()} Conntect: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task OpenHistory()
    {
        OpenLastConnection = !OpenLastConnection;
    }

    [RelayCommand]
    public async Task SetHistoryServer(ServerConnection serveradress)
    {
        InputserverAddress = serveradress.ServerAdress;
        Username = serveradress.UserName;
    }


}
