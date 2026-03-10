using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels
{
    public class ConnectViewModel: PageViewModelBase
    {
        private AppState appState;
        public string Username { get; set; } = "";
        public string ServerAddress { get; set; } = "";
        public override bool CanNavigateNext { get => true; protected set => throw new NotImplementedException(); }
        public override bool CanNavigatePrevious { get => true; protected set => throw new NotImplementedException(); }
        public ICommand ConnectCommand { get; }

        public ConnectViewModel(AppState appState) 
        {
            ConnectCommand = ReactiveCommand.Create(connect);
            this.appState = appState;
            Username = appState.ClientData.UserData.UserName;
            ServerAddress = appState.ClientData.FavServers?.FirstOrDefault()?.ServerAdress??"";
        }


        private async Task connect()
        {
            try
            {
                appState.ClientData.UserData = new AppStateUser(appState.ClientData.UserData.ClientId, Username);
                appState.ClientData.FavServers = new List<FavServer>() { new FavServer(Username, ServerAddress)};
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
