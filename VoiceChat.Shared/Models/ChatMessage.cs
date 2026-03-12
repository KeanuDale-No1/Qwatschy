namespace VoiceChat.Shared.Models;

public class ChatMessageDTO
{
    public string Username { get; set; } = "";
    public string Text { get; set; } = "";
    public DateTime Timestamp { get; set; }
}
