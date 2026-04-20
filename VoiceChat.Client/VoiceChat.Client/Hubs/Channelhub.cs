using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Client.Models;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Client.Hubs;

public partial class ClientHub
{

    private void RegisterChannelEventHandlers(HubConnection connection, Guid serverId)
    {
        connection.On<ChannelDTO>("ChannelAdded", channel =>
        {
            //TODO: Add channel to the corresponding server's channel list
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
    }

    public async Task AddChannel(Guid serverId, ChannelInfo channel)
    {
       if (_connections.TryGetValue(serverId, out var connection) && connection.State == HubConnectionState.Connected)
        {
            await connection.InvokeAsync("AddChannel", new ChannelDTO(Guid.NewGuid(), channel.Name,channel.Desciption,null));
        }
    }

}
