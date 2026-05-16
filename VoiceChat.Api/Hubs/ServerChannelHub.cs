using Microsoft.AspNetCore.SignalR;
using VoiceChat.Data.Repositories;
using VoiceChat.Domain.Channel;
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Api.Hubs
{
    public partial class ServerHub
    {
        public async Task AddChannel(ChannelDTO channel)
        {
            // Logik zum Hinzufügen eines Kanals
            await channelRepository.AddAsync(new() { Id = channel.Id, Name = channel.Name, Descripton = channel.Description, CreatedAt = DateTime.UtcNow });
            await channelRepository.SaveAsync();
            var channeldb = await channelRepository.GetByIdAsync(channel.Id);
            // Optional: Benachrichtigung an alle Clients über den neuen Kanal
            await Clients.All.SendAsync("ChannelAdded", new ChannelDTO(channeldb.Id, channeldb.Name, channeldb.Descripton, new ConnectedUserDTO[0]));
        }

        public async Task DeleteChannel(Guid id)
        {
            var channel = await channelRepository.GetByIdAsync(id);
            if (channel != null)
            {
                await channelRepository.DeleteAsync(channel);
                await channelRepository.SaveAsync();
            }
            await Clients.All.SendAsync("ChannelDeleted", id);
        }
    }
}
