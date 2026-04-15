using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services.AppSettings;

namespace VoiceChat.Client.Services.ServerService;

internal class ServerService(IAppSettingsService appSettingsService, ClientHub clientHub) : IServerService
{

    public async Task<bool> AddServer(string serverAdress)
    {
        Guid serverId = Guid.NewGuid();
        await clientHub.ConnectAsync(serverId, serverAdress);
        if (clientHub.ConnectedServers.Contains(serverId))
        {
            appSettingsService.AddServer(serverId, serverAdress);
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task ConnectAll()
    {
        var servers = appSettingsService.AppSetting.Servers.ServerAddresses;

        foreach (var server in servers)
        {

            await clientHub.ConnectAsync(server.ServerId, server.ServerAddress);
        }


    }
}
