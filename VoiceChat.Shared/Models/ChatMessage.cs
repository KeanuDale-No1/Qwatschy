namespace VoiceChat.Shared.Models;



public record CreateChatMessageRequestDTO(ChatMessageDTO message);
public record CreateChatMessageResponseDTO(ChatMessageDTO message);

public record GetMessagesRequestDTO(Guid ChannelId, int Skip = 0, int Take = 40);
public record GetMessagesResponseDTO(List<ChatMessageDTO> Messages, int TotalCount);

public record ChatMessageDTO(Guid SenderId, Guid ChannelId, string Content, DateTime Timestamp, string? Username = "");