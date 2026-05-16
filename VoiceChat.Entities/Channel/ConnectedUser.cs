using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Domain.Channel;

internal class ConnectedUser
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
}
