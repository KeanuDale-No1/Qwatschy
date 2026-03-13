using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Domain.Channel
{
    public class Message:EntityBase
    {
        public Guid ChannelId { get; set; }
        public Guid UserId { get; set; }
        public required string Text { get; set; }
    }
}
