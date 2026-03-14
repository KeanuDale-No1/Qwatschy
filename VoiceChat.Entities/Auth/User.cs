using VoiceChat.Domain;

namespace VoiceChat.Domain.Auth;

public class User : EntityBase
{
    public string DisplayName { get; set; } = "User";
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public Guid? ConnectedChannel { get; set; }

}
