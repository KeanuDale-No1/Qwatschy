using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using VoiceChat.Client.Models;
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Client.Hubs;

public partial class ClientHub
{
    private void RegisterChannelEventHandlers(HubConnection connection, Guid serverId)
    {
        connection.On<ChannelDTO>("ChannelAdded", channel =>
        {
            var serverinfo = ServerConnectionInfos.FirstOrDefault(s => s.ServerId == serverId);
            serverinfo?.ChannelInfos.Add(new ChannelInfo()
            {
                Id = channel.Id,
                Name = channel.Name,
                Desciption = channel.Description,
                UnreadMessagesCount = 0,
                ConnectedUsers = new()
            });
            Console.WriteLine($"[ChannelHub] Channel added: {channel.Name} (ID: {channel.Id})");
        });

        connection.On<Guid>("ChannelDeleted", id =>
        {
            var serverinfo = ServerConnectionInfos.FirstOrDefault(s => s.ServerId == serverId);
            var channel = serverinfo?.ChannelInfos.FirstOrDefault(c => c.Id == id);
            if (channel != null)
                serverinfo?.ChannelInfos.Remove(channel);

            Console.WriteLine($"[ChannelHub] Channel deleted: {channel?.Name} (ID: {id})");
        });
    }

    public async Task AddChannel(Guid serverId, ChannelInfo channel)
    {
        if (_connections.TryGetValue(serverId, out var connection) && connection.State == HubConnectionState.Connected)
        {
            await connection.InvokeAsync("AddChannel", new ChannelDTO(Guid.NewGuid(), channel.Name, channel.Desciption, null));
        }
    }

    public async Task DeleteChannel(Guid serverId, ChannelInfo channel)
    {
        if (_connections.TryGetValue(serverId, out var connection) && connection.State == HubConnectionState.Connected)
        {
            await connection.InvokeAsync("DeleteChannel", channel.Id);
        }
    }

}
