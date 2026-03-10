using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VoiceChat.Client.Services
{
    public class AppState
    {
        private string usersettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + VoiceChatConstants.VoiceChatName + "/usersettings.dat";
        public AppState() { }

        public ClientData ClientData { get; set; } = new ClientData();

        public void SaveClientData()
        {
            var json = JsonSerializer.Serialize(ClientData);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            File.WriteAllText(usersettingsPath, json);
        }

        public void RestoreClientData()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!File.Exists(usersettingsPath))
                return;
            var json = File.ReadAllText(usersettingsPath);
            var clientData = JsonSerializer.Deserialize<ClientData>(json);
            if (clientData != null)
                ClientData = clientData;
        }
    }

    public record ClientData
    {
        public AppStateUser UserData { get; set; } = new AppStateUser(Guid.NewGuid(), "User");
        public List<FavServer> FavServers { get; set; } = new List<FavServer>();

        [JsonIgnore]
        public string JwtToken { get; set; }
    }

    public record AppStateUser(Guid ClientId, string UserName);

    public record FavServer(string UserName, string ServerAdress);

}
