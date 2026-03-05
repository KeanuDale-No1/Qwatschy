namespace VoiceChat.Shared.Models;

public class ChatMessage
{
    public string Username { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTime Timestamp { get; set; }
}
