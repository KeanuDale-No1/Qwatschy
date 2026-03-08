using VoiceChat.Domain;

namespace VoiceChat.Domain.Auth;

public class User : EntityBase
{
    public string? DisplayName { get; set; }
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
}
