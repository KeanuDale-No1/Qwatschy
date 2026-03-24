using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VoiceChat.Client.Services
{
    public class AppState
    {
        public IApplicationLifetime? ApplicationLifetime { get; set; }
        private string folder = "";
        private string filePath = "";
        private bool isDesktop = false;
        
        public AppState(IApplicationLifetime? applicationLifetime)
        {
            ApplicationLifetime= applicationLifetime;
            isDesktop = applicationLifetime is IClassicDesktopStyleApplicationLifetime;
            
            if (isDesktop)
            {
                try
                {
                    folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), QwatschyConstants.VoiceChatName);
                    filePath = System.IO.Path.Combine(folder, "usersettings.dat");
                }
                catch
                {
                    isDesktop = false;
                }
            }
            
            if (isDesktop)
            {
                RestoreClientData();
            }
        }

        private ClientData ClientData { get; set; } = new ClientData();
        public AppStateUser GetUser() => ClientData.UserData;

        
        public void SetUsername(string username)
        {
            ClientData.UserData.UserName = username;
            SaveClientData();
        }


        public ServerConnection? GetLastServer() => ClientData.ServerConnections.LastOrDefault();
        public List<ServerConnection> GetLastServers() => ClientData.ServerConnections;

        public void AddServer(ServerConnection connection)
        {
            if (ClientData.ServerConnections.Any(c => c.ServerAdress == connection.ServerAdress && c.UserName == connection.UserName))
                return;
            ClientData.ServerConnections.Add(connection);
            SaveClientData();
        }
        public void RemoveServer(ServerConnection connection)
        {
            ClientData.ServerConnections.Remove(connection);
            SaveClientData();
        }
        public void ClearServerConnections()
        {
            ClientData.ServerConnections.Clear();
            SaveClientData();
        }

        private void SaveClientData()
        {
            if (!isDesktop) return;
            
            try
            {
                var json = JsonSerializer.Serialize(ClientData, ClientDataContext.Default.ClientData);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                File.WriteAllText(filePath, json);
            }
            catch { }
        }

        private void RestoreClientData()
        {
            if (!isDesktop) return;
            
            try
            {
                if (!File.Exists(filePath))
                    return;
                var json = File.ReadAllText(filePath);
                var clientData = JsonSerializer.Deserialize<ClientData>(json);
                if (clientData != null)
                    ClientData = clientData;
            }
            catch { }
        }
    }

    public class ClientData
    {
        public AppStateUser UserData { get; set; } = new AppStateUser(Guid.NewGuid(), "User");

        public List<ServerConnection> ServerConnections { get; set; } = new List<ServerConnection>();
    }

    public class AppStateUser
    {
        public AppStateUser(Guid clientId, string userName)
        {
            ClientId = clientId;
            UserName = userName;
        }

        public Guid ClientId { get; private set; }
        public string UserName { get; set; }
    };

    public record ServerConnection(string UserName, string ServerAdress);



    [JsonSerializable(typeof(ClientData))]
    [JsonSerializable(typeof(AppStateUser))]
    [JsonSerializable(typeof(ServerConnection))]
    public partial class ClientDataContext : JsonSerializerContext
    {
    }

}
