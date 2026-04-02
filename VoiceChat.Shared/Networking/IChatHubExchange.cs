using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Shared.Models;

namespace VoiceChat.Shared.Networking
{
    public interface IChatHubExchange
    {
        //Server Management
        Task OnConnectedAsync();

        //Channel Management
        Task JoinChannel(Guid channelId);
        Task AddChannel(ChannelDTO message);
        Task DeleteChannel(Guid channelId);

        //Chat Managment 
        Task SendMessage(ChatMessageDTO message);
        Task<GetMessagesResponseDTO> GetMessages(Guid channelId, int skip = 0, int take = 50);

        //User Management
        Task KickUser(Guid channelId, Guid userId);
        Task BanUser(Guid channelId, Guid userId);
    }
}
