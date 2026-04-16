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
using VoiceChat.Client.Services;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Hubs;

public partial class ServerConnectionInfo : ObservableObject
{
    public ServerConnectionInfo(Guid serverId, string serverAdress, string serverName)
    {
        ServerId = serverId;
        ServerAdress = serverAdress;
        ServerName = serverName;
    }

    public Guid ServerId { get; set; }
    public string ServerAdress { get; set; }
    public string ServerName { get; set; }
    public string? ServerImage { get; set; }
    public string? Token { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    [ObservableProperty] private bool isConnected;

    public string Abbr => AbbreviationHelper.GetAbbreviation(ServerName);
}

public static class AbbreviationHelper
{
    public static string GetAbbreviation(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return new string(input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length > 2)
            .SelectMany(w => w.Where(char.IsUpper))
            .Take(3)
            .ToArray());
    }
}

public class ClientHub(IAppSettingsService appSettingsService, ITokenProvider tokenProvider) : IClientHubExchange, IDisposable
{
    private readonly ConcurrentDictionary<Guid, HubConnection> _connections = new();
    public readonly ObservableCollection<ServerConnectionInfo> ServerConnectionInfos = new();


    public event Action<Guid, ChatMessageDTO>? MessageReceived;
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

    public async Task SendMessageAsync(Guid serverId, ChatMessageDTO message)
    {
        if (_connections.TryGetValue(serverId, out var connection) && connection.State == HubConnectionState.Connected)
        {
            await connection.InvokeAsync("SendMessage", message);
        }
    }

    public async Task<GetMessagesResponseDTO> GetMessagesAsync(Guid serverId, Guid channelId, int skip = 0, int take = 50)
    {
        if (_connections.TryGetValue(serverId, out var connection) && connection.State == HubConnectionState.Connected)
        {
            return await connection.InvokeAsync<GetMessagesResponseDTO>("GetMessages", channelId, skip, take);
        }

        return new GetMessagesResponseDTO(new List<ChatMessageDTO>(), 0);
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

        connection.On<ChatMessageDTO>("ReceiveMessage", (message) =>
        {
            MessageReceived?.Invoke(serverId, message);
        });

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