using VoiceChat.Data.Repositories;
using VoiceChat.Domain.Channel;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.UseCases.Channels;

public class CreateChannelUseCase(IRepository<Channel> repository) : IUseCase<CreateChannelRequestDTO, CreateChannelResponseDTO>
{
    public async Task<CreateChannelResponseDTO> ExecuteAsync(CreateChannelRequestDTO request)
    {
        if (request.Channel == null)
            throw new ArgumentNullException(nameof(request.Channel));
        
        var channel = new Channel() {  Id = request.Channel.Id, Name = request.Channel.Name, Descripton= request.Channel.Description, CreatedAt = DateTime.UtcNow};

        await repository.AddAsync(channel);
        await repository.SaveAsync();

        return new CreateChannelResponseDTO(new ChannelDTO() { Id = channel.Id, Name = channel.Name, Description = channel.Descripton });
    }
}
