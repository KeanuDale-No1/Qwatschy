namespace VoiceChat.Api.Options;

public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int Expires { get; set; }
    public string SecretKey { get; set; } = string.Empty;
}
