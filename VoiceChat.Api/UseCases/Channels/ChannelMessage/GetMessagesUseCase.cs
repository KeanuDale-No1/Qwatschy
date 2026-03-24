using VoiceChat.Data.Repositories;
using VoiceChat.Domain.Auth;
using VoiceChat.Domain.Channel;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.UseCases.Channels.ChannelMessage;

public class GetMessagesUseCase(IRepository<Message> repository, IRepository<User> userRepository) : IUseCase<GetMessagesRequestDTO, GetMessagesResponseDTO>
{
    public async Task<GetMessagesResponseDTO> ExecuteAsync(GetMessagesRequestDTO request)
    {
        var messages = await repository.GetAllAsync();
        var channelMessages = messages
            .Where(m => m.ChannelId == request.ChannelId)
            .OrderByDescending(m => m.CreatedAt)
            .ToList();

        var totalCount = channelMessages.Count;
        
        var pagedMessages = channelMessages
            .Skip(request.Skip)
            .Take(request.Take)
            .OrderBy(m => m.CreatedAt)
            .ToList();

        var messageDtos = new List<ChatMessageDTO>();
        foreach (var msg in pagedMessages)
        {
            var user = await userRepository.GetByIdAsync(msg.SenderId);
            messageDtos.Add(new ChatMessageDTO(msg.SenderId, msg.ChannelId, msg.Content, msg.CreatedAt, user?.Username ?? "Unknown"));
        }

        return new GetMessagesResponseDTO(messageDtos, totalCount);
    }
}
