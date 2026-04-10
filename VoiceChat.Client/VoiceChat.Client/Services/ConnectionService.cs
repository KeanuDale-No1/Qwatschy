using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Client.ViewModels.Login;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Services
{
    public class ConnectionService(ApiService apiService,
                                 INavigationService navigationService,
                                 IAppSettingsService appState,
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
            stateService.SetConnectedServer(appState.AppSetting.UserSettings.UserId, username, serveraddress);
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO(appState.AppSetting.UserSettings.UserId, appState.AppSetting.UserSettings.Username);
            var response = await apiService.PostAsync<LoginRequestDTO, LoginResponseDTO>("/api/login", loginRequestDTO);

            if (response != null && response.UserId != Guid.Empty && !String.IsNullOrEmpty(response.AuthToken))
            {
                tokenService.WriteNewToken(response.AuthToken);
                await serviceHub.OnConnectedAsync();
                await navigationService.NavigateTo<MainAreaViewModel>();
            }
        }
    }
}
