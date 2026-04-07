using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.ViewModels.Login;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Services
{
    public class ConnectionService(ApiService apiService,
                                 INavigationService navigationService,
                                 AppState appState,
                                 StateService stateService,
                                 TokenService tokenService, 
                                 ChatHubClient serviceHub,
                                 VoiceHubClient voiceHubClient)
    {


        public async Task ServerDisconnect()
        {
            await serviceHub.Disconnect();
            await voiceHubClient.LeaveChannelAsync();

            tokenService.WriteNewToken("");
            stateService.SetDisconnectedServer();
            await navigationService.NavigateTo<LoginViewModel>();
        }

        public async Task ServerConnect(string username, string serveraddress)
        {
            appState.SetUsername(username);
            stateService.SetConnectedServer(appState.GetUser().ClientId, username, serveraddress);
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO(appState.GetUser().ClientId, appState.GetUser().UserName);
            var response = await apiService.PostAsync<LoginRequestDTO, LoginResponseDTO>("/api/login", loginRequestDTO);

            if (response != null && response.UserId != Guid.Empty && !String.IsNullOrEmpty(response.AuthToken))
            {
                appState.AddServer(new ServerConnection(appState.GetUser().UserName, serveraddress));
                tokenService.WriteNewToken(response.AuthToken);
                await serviceHub.OnConnectedAsync();
                await navigationService.NavigateTo<MainAreaViewModel>();
            }
        }
    }
}
