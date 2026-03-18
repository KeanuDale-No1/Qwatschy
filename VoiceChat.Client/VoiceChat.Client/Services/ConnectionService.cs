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
    public class ConnectionService(IHttpClientService httpService,
                                 INavigationService navigationService,
                                 AppState appState,
                                 StatusService statusService,
                                 TokenService tokenService, ServiceHubClient serviceHub)
    {
        

        private string serverAddress;
        public Guid? ConnectedChannelId { get; internal set; }
        
        public async Task ServerDisconnect()
        {
            statusService.AddReport("Versuche verbindung zu trennen...");
            await httpService.CreateClient("");
            await navigationService.NavigateTo<LoginViewModel>();
        }

        public async Task ServerConnect(string serveraddress) 
        {
            try
            {
                serverAddress = serveraddress;
                statusService.AddReport("Versuche zu verbinden...");

                await httpService.CreateClient(serveraddress);
                LoginRequestDTO loginRequestDTO = new LoginRequestDTO(appState.GetUser().ClientId, appState.GetUser().UserName);
                var response = await httpService.PostAsync<LoginRequestDTO, LoginResponseDTO>("/api/login", loginRequestDTO);

                if (response != null && response.UserId != Guid.Empty && !String.IsNullOrEmpty(response.AuthToken))
                {
                    appState.AddServer(new ServerConnection(appState.GetUser().UserName, serveraddress));
                    tokenService.WriteNewToken(response.AuthToken);
                    await httpService.CreateClient(serveraddress);
                    await serviceHub.Connect(serveraddress);
                    await navigationService.NavigateTo<MainAreaViewModel>();
                }
                else
                {
                    statusService.AddReport("Verbindung konnte nicht hergestellt werden.");
                }

            }
            catch (Exception ex)
            {
                statusService.AddReport(ex.Message);
            }
        }


        public void ChannelConnect(Guid channelId)
        {
           this.ConnectedChannelId = channelId;
        }

        public void ChannelDisconnect() 
        {
            ConnectedChannelId = null;
        }


    }
}
