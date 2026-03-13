namespace VoiceChat.Domain.Channel;

public class Channel : EntityBase
{
    public required string Name { get; set; }
    public string? Descripton { get; set; }
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
