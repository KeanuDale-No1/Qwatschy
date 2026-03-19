using VoiceChat.Data.Repositories;
using VoiceChat.Domain.Auth;
using VoiceChat.Domain.Channel;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.UseCases.Channels.ChannelMessage
{
    public class CreateChatMessageUseCase(IRepository<Message> repository, IRepository<User> userRepository) : IUseCase<CreateChatMessageRequestDTO, CreateChatMessageResponseDTO>
    {
        public async Task<CreateChatMessageResponseDTO> ExecuteAsync(CreateChatMessageRequestDTO request)
        {
            var message = new Message
            {
                SenderId = request.message.SenderId,
                ChannelId = request.message.ChannelId,
                Content = request.message.Content,
            };
            await repository.AddAsync(message);
            await repository.SaveAsync();

            var user = await userRepository.GetByIdAsync(message.SenderId);

            return new CreateChatMessageResponseDTO(new ChatMessageDTO(message.SenderId, message.SenderId,message.Content,message.CreatedAt, user.Username));
        }
    }
}
