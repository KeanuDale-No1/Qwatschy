using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Shared.Models;

public class LoginRequest
{
    public Guid ClientId { get; set; } = Guid.Empty;
    public string? DisplayName { get; set; }
}

public class LoginResponse
{
    public Guid UserId { get; set; } = Guid.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
}