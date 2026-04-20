using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Client.Hubs;

public partial class ClientHub
{


    //public async Task SendMessageAsync(Guid serverId, ChatMessageDTO message)
    //{
    //    if (_connections.TryGetValue(serverId, out var connection) && connection.State == HubConnectionState.Connected)
    //    {
    //        await connection.InvokeAsync("SendMessage", message);
    //    }
    //}

    //public async Task<GetMessagesResponseDTO> GetMessagesAsync(Guid serverId, Guid channelId, int skip = 0, int take = 50)
    //{
    //    if (_connections.TryGetValue(serverId, out var connection) && connection.State == HubConnectionState.Connected)
    //    {
    //        return await connection.InvokeAsync<GetMessagesResponseDTO>("GetMessages", channelId, skip, take);
    //    }

    //    return new GetMessagesResponseDTO(new List<ChatMessageDTO>(), 0);
    //}

    private void RegisterChatEventHandlers(HubConnection connection)
    {
    }
}
