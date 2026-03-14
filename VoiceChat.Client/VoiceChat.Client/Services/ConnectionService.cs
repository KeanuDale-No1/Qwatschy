using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Client.ViewModels.Login;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Services
{
    public class ConnectionService
    {
        private readonly AppState appState;
        private readonly StatusService statusService;
        private readonly IHttpClientService httpService;
        private readonly INavigationService navigationService;
        private readonly ChatService chatService;
        public ConnectionService(IHttpClientService httpService, 
                                 INavigationService navigationService, 
                                 AppState appState, 
                                 StatusService statusService,
                                 ChatService chatService) 
        {
            this.appState = appState;
            this.statusService = statusService;
            this.httpService = httpService;
            this.navigationService = navigationService;
            this.chatService = chatService;
        }

        private string serverAdresss;

        public async Task ServerDisconnect()
        {
            statusService.AddReport("Versuche verbindung zu trennen...");
            await httpService.CreateClient("", "");
            await navigationService.NavigateTo<LoginViewModel>();
        }

        public async Task ServerConnect(string username, string serveradress) 
        {
            try
            {
                serverAdresss = serveradress;
                appState.SetUsername(username);
                var user = appState.GetUser();
                statusService.AddReport("Versuche zu verbinden...");

                await httpService.CreateClient(serveradress, "");
                LoginRequestDTO loginRequestDTO = new LoginRequestDTO(user.ClientId, user.UserName);
                var response = await httpService.PostAsync<LoginRequestDTO, LoginResponseDTO>("/api/login", loginRequestDTO);
                if (response != null)
                {
                    statusService.AddReport("Verbindung erfolgreich hergestellt.");
                    appState.AddServer(new ServerConnection(username, serveradress));
                    await httpService.CreateClient(serveradress, response.AuthToken);
                    await chatService.Connect(serveradress);
                    await navigationService.NavigateTo<MainAreaViewModel>();
                }

            }
            catch (Exception ex)
            {
                statusService.AddReport(ex.Message);
            }
        }


        public async Task ChannelConnect()
        {

        }

        public async Task ChannelDisconnect() 
        { 
            
        }


    }
}
