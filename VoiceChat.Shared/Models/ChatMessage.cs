namespace VoiceChat.Shared.Models;



public record CreateChatMessageRequestDTO(ChatMessageDTO message);
public record CreateChatMessageResponseDTO(ChatMessageDTO message);

public record ChatMessageDTO(Guid SenderId, Guid ChannelId, string Content, DateTime Timestamp, string? Username = "");