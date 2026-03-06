namespace VoiceChat.Entities;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? DisplayName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
}
