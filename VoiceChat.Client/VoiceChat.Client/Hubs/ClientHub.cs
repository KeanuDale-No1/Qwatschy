using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Models;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Client.Services.ServerViewService;
using VoiceChat.Client.Services.TokenProviders;

namespace VoiceChat.Client.Hubs;




public partial class ClientHub(IAppSettingsService appSettingsService, ITokenProvider tokenProvider, IServerViewService serverViewService) : IClientHubExchange, IDisposable
{
    private readonly ConcurrentDictionary<Guid, HubConnection> _connections = new();
    public readonly ObservableCollection<ServerConnectionInfo> ServerConnectionInfos = new();

    public event Action<Guid, HubConnectionState>? ConnectionStateChanged;

    public IEnumerable<Guid> ConnectedServers => _connections.Keys;

    public async Task AddServerAsync(string serverAddress)
    {
        var serverId = Guid.NewGuid();
        await ConnectAsync(serverId, serverAddress);
        appSettingsService.AddServer(serverId, serverAddress);
    }

    public async Task RemoveServerAsync(Guid serverId)
    {
        if (serverViewService.OpenServerInfo.ServerId == serverId)
            serverViewService.UpdateServerConnectionInfo(null);
        await DisconnectAsync(serverId);
        var serverInfo = ServerConnectionInfos.FirstOrDefault(s => s.ServerId == serverId);
        if (serverInfo != null)
            ServerConnectionInfos.Remove(serverInfo);
        appSettingsService.RemoveServer(serverId);
    }

    public async Task ConnectAllAsync()
    {
        var servers = appSettingsService.AppSetting.Servers.ServerAddresses;
        var userSettings = appSettingsService.AppSetting.UserSettings;
        foreach (var server in servers)
        {
            await ConnectAsync(server.ServerId, server.ServerAddress, userSettings.UserId, userSettings.Username);
        }
    }

    public async Task ConnectAsync(Guid serverId, string serverAddress)
    {
        var userSettings = appSettingsService.AppSetting.UserSettings;
        await ConnectAsync(serverId, serverAddress, userSettings.UserId, userSettings.Username);
    }

    public async Task ConnectAsync(Guid serverId, string serverAddress, Guid clientId, string? displayName)
    {
        if (!string.IsNullOrWhiteSpace(serverAddress) && !_connections.ContainsKey(serverId))
        {
            var serverInfo = new ServerConnectionInfo(serverId, serverAddress, "Connecting...");

            var serverData = await tokenProvider.GetServerInfoAsync(serverAddress, clientId, displayName);
            if (serverData != null)
            {
                serverInfo.Token = serverData.Token;
                serverInfo.ServerName = serverData.ServerName;
                serverInfo.ServerImage = serverData.ServerImage;
                serverInfo.Description = serverData.Description;
                serverInfo.ChannelInfos = new ObservableCollection<ChannelInfo>(serverData.Channels.Select(c => new ChannelInfo
                {
                    Id = c.Id,
                    Name = c.Name,
                    Desciption = c.Description
                }));
            }
            else
            {
                serverInfo.ServerName = new Uri(serverAddress).Host;
            }
            ServerConnectionInfos.Add(serverInfo);

            await ConnectToServerAsync(serverInfo);
        }
    }

    public async Task DisconnectAsync(Guid serverId)
    {
        await DisconnectServerAsync(serverId);
    }


    private void RegisterEventHandlers(HubConnection connection, Guid serverId)
    {
        RegisterChatEventHandlers(connection, serverId);
        RegisterChannelEventHandlers(connection, serverId);
    }


    private async Task ConnectToServerAsync(ServerConnectionInfo serverConnectionInfo)
    {
        var serverId = serverConnectionInfo.ServerId;
        var serverAddress = serverConnectionInfo.ServerAdress;
        var url = serverAddress.TrimEnd('/') + "/connection";

        var builder = new HubConnectionBuilder()
            .WithUrl(url, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(serverConnectionInfo.Token ?? string.Empty);
                options.Transports = HttpTransportType.LongPolling;
            })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions = Json.Options;
            })
            .WithAutomaticReconnect();

        var connection = builder.Build();


        connection.Closed += async (exception) =>
        {
            serverConnectionInfo.IsConnected = false;
            ConnectionStateChanged?.Invoke(serverId, HubConnectionState.Disconnected);
            await Task.CompletedTask;
        };

        connection.Reconnecting += async (exception) =>
        {
            serverConnectionInfo.IsConnected = false;
            ConnectionStateChanged?.Invoke(serverId, HubConnectionState.Reconnecting);
            await Task.CompletedTask;
        };

        connection.Reconnected += async (connectionId) =>
        {
            serverConnectionInfo.IsConnected = true;
            ConnectionStateChanged?.Invoke(serverId, HubConnectionState.Connected);
            await Task.CompletedTask;
        };


        RegisterEventHandlers(connection, serverId);

        _connections[serverId] = connection;
        ConnectionStateChanged?.Invoke(serverId, HubConnectionState.Connecting);

        try
        {
            await connection.StartAsync();
            serverConnectionInfo.IsConnected = true;
            ConnectionStateChanged?.Invoke(serverId, HubConnectionState.Connected);
        }
        catch (Exception ex)
        {
            serverConnectionInfo.ErrorMessage = ex.Message;
            serverConnectionInfo.IsConnected = false;
            ConnectionStateChanged?.Invoke(serverId, HubConnectionState.Disconnected);
        }
    }

    private async Task DisconnectServerAsync(Guid serverId)
    {
        if (_connections.TryRemove(serverId, out var connection))
        {
            await connection.StopAsync();
            await connection.DisposeAsync();
        }

        ConnectionStateChanged?.Invoke(serverId, HubConnectionState.Disconnected);
    }

    public void Dispose()
    {
        foreach (var connection in _connections.Values)
        {
            connection.DisposeAsync();
        }
        _connections.Clear();
    }
}