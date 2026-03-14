namespace VoiceChat.Shared.Models;

public class ChatMessageDTO
{
    public Guid ClientId { get; set; } = Guid.Empty;
    public Guid ChannelId { get; set; }
    public string Username { get; set; } = "";
    public string Text { get; set; } = "";
    public DateTime Timestamp { get; set; }
}
