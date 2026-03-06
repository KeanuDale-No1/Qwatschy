using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Domain
{
    public abstract class EntityBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
