using System;

namespace VoiceChat.Client.Models;

public class ConnectedUser
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
}