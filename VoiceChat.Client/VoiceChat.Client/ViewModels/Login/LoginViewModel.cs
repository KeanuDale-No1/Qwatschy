using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.Login;

public partial class LoginViewModel : ViewModelBase
{

    private readonly IHttpClientService httpClient;
    private AppState appState;
    INavigationService navigationService;
    public string Username { get; set; } = "";
    public string ServerAddress { get; set; } = "";


    private readonly ChatService chatService;
    public LoginViewModel(AppState appState, INavigationService navigationService, ChatService chatService, IHttpClientService httpClient)
    {
        this.httpClient = httpClient;
        this.appState = appState;
        this.navigationService = navigationService;
        this.chatService = chatService;
        Username = appState.ClientData.UserData.UserName;
        ServerAddress = appState.ClientData.FavServers?.FirstOrDefault()?.ServerAdress ?? "";
    }

    [RelayCommand]
    public async Task Conntect()
    {
        try
        {
            appState.ClientData.UserData = new AppStateUser(appState.ClientData.UserData.ClientId, Username);
            appState.ClientData.FavServers = new List<FavServer>() { new FavServer(Username, ServerAddress) };
            appState.ServerAdress = ServerAddress;

            appState.SaveClientData();
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO(appState.ClientData.UserData.ClientId, appState.ClientData.UserData.UserName);

            var response = await httpClient.PostAsync<LoginRequestDTO, LoginResponseDTO>("/api/login", loginRequestDTO);
            if (response != null)
            {
                appState.ClientData.JwtToken = response.AuthToken;
                await chatService.Connect();
                await navigationService.NavigateTo<MainAreaViewModel>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }



}
