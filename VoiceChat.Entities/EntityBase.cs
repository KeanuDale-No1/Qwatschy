using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Domain
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
