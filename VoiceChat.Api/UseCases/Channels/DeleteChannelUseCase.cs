using VoiceChat.Data.Repositories;
using VoiceChat.Domain.Channel;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.UseCases.Channels;

public class DeleteChannelUseCase(IRepository<Channel> repository) : IUseCase<DeleteChannelRequestDTO, DeleteChannelResponseDTO>
{
    public async Task<DeleteChannelResponseDTO> ExecuteAsync(DeleteChannelRequestDTO request)
    {
        var channel = await repository.GetByIdAsync(request.channelId);
        if (channel == null)
            return new DeleteChannelResponseDTO(false);

        await repository.DeleteAsync(channel);

        await repository.SaveAsync();
        return new DeleteChannelResponseDTO(true);
    }
}
