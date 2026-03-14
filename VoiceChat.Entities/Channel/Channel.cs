using VoiceChat.Domain.Auth;

namespace VoiceChat.Domain.Channel;

public class Channel : EntityBase
{
    public required string Name { get; set; }
    public string? Descripton { get; set; }
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    public virtual ICollection<User> Users{ get; set; } = new List<User>();

}
