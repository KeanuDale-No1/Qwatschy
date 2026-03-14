namespace VoiceChat.Shared.Models;



public record CreateChannelRequestDTO(ChannelDTO Channel);
public record CreateChannelResponseDTO(ChannelDTO Channel);



public record GetChannelsRequestDTO();
public record GetChannelsResponseDTO(ChannelDTO[] Channels);


public record DeleteChannelRequestDTO(Guid channelId);
public record DeleteChannelResponseDTO(bool isDelete);




public class ChannelDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public bool UnreadCount { get; set; }
}
