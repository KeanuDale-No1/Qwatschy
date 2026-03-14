using VoiceChat.Data.Repositories;
using VoiceChat.Domain.Auth;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.UseCases.Channels
{
    public class ConnectChannelUseCase(IRepository<User> repository) : IUseCase<ConnectChannelRequestDTO, ConnectChannelResponseDTO>
    {
        public async Task<ConnectChannelResponseDTO> ExecuteAsync(ConnectChannelRequestDTO request)
        {
            var user = await repository.GetByIdAsync(request.UserId);
            user.ConnectedChannel = request.ChannelId;
            await repository.UpdateAsync(user);
            await repository.SaveAsync();
            return new ConnectChannelResponseDTO(user.Id, user.DisplayName, request.ChannelId);
        }
    }
}
