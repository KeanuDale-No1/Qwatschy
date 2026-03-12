using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VoiceChat.Client.Services
{
    public class AppState
    {
        public IApplicationLifetime? ApplicationLifetime { get; set; }
        private string folder = "";
        private string filePath = "";
        public string ServerAdress { get; set; }
        public AppState()
        {
            folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), QwatschyConstants.VoiceChatName);
            filePath = System.IO.Path.Combine(folder, "usersettings.dat");
        }

        public ClientData ClientData { get; set; } = new ClientData();

        public void SaveClientData()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var json = JsonSerializer.Serialize(ClientData, ClientDataContext.Default.ClientData);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                File.WriteAllText(filePath, json);
            }
        }

        public void RestoreClientData()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (!File.Exists(filePath))
                    return;
                var json = File.ReadAllText(filePath);
                var clientData = JsonSerializer.Deserialize<ClientData>(json);
                if (clientData != null)
                    ClientData = clientData;
            }
        }
    }

    [JsonSerializable(typeof(ClientData))]
    [JsonSerializable(typeof(AppStateUser))]
    [JsonSerializable(typeof(FavServer))]
    public record ClientData
    {
        public AppStateUser UserData { get; set; } = new AppStateUser(Guid.NewGuid(), "User");

        public List<FavServer> FavServers { get; set; } = new List<FavServer>();

        [JsonIgnore]
        public string JwtToken { get; set; }
    }

    public record AppStateUser(Guid ClientId, string UserName);

    public record FavServer(string UserName, string ServerAdress);



    [JsonSerializable(typeof(ClientData))]
    [JsonSerializable(typeof(AppStateUser))]
    [JsonSerializable(typeof(FavServer))]
    public partial class ClientDataContext : JsonSerializerContext
    {
    }

}
