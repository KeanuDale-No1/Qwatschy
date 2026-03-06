using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Shared.Models;

public class LoginRequest
{
    public string ClientId { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
}

public class LoginResponse
{
    public string UserId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
}