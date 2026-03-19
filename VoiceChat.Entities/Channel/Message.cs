using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Domain.Channel
{
    public class Message:EntityBase
    {
        public Guid SenderId { get; set; }
        public Guid ChannelId { get; set; }
        public required string Content { get; set; }
    }
}
