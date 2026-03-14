using VoiceChat.Data.Repositories;
using VoiceChat.Domain.Channel;
using VoiceChat.Shared.Models;


namespace VoiceChat.Api.UseCases.Channels
{
    public class GetChannelUseCase(IRepository<Channel> repository) : IUseCase<GetChannelsRequestDTO, GetChannelsResponseDTO>
    {
        public async Task<GetChannelsResponseDTO> ExecuteAsync(GetChannelsRequestDTO request)
        {
            var channels = await repository.GetAllAsync();


            return new GetChannelsResponseDTO(channels?.Select(c => new ChannelDTO() { Id = c.Id, Description = c.Descripton, Name = c.Name }).ToArray() ?? new ChannelDTO[0]);

        }
    }
}
