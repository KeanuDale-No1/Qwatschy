using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.Login;

public partial class LoginViewModel: ViewModelBase
{
    private AppState appState;
    INavigationService navigationService;
    public string Username { get; set; } = "";
    public string ServerAddress { get; set; } = "";


    private readonly ChatService chatService;
    public LoginViewModel(AppState appState, INavigationService navigationService, ChatService chatService) 
    {
        this.appState = appState;
        this.navigationService = navigationService;
        this.chatService = chatService;
        Username = appState.ClientData.UserData.UserName;
        ServerAddress = appState.ClientData.FavServers?.FirstOrDefault()?.ServerAdress??"";
    }

    [RelayCommand]
    public async Task Conntect()
    {
        try
        {
            appState.ClientData.UserData = new AppStateUser(appState.ClientData.UserData.ClientId, Username);
            appState.ClientData.FavServers = new List<FavServer>() { new FavServer(Username, ServerAddress)};
            appState.ServerAdress = ServerAddress;

             appState.SaveClientData();
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO(appState.ClientData.UserData.ClientId, appState.ClientData.UserData.UserName);

            var client = new HttpClient();

            client.BaseAddress = new Uri(ServerAddress);
            var response = await client.PostAsJsonAsync("/api/login", loginRequestDTO);
            if (response != null)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
                if (loginResponse != null) {
                    appState.ClientData.JwtToken = loginResponse.AuthToken;
                    await chatService.Connect();
                    await navigationService.NavigateTo<MainAreaViewModel>();

                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    
}
