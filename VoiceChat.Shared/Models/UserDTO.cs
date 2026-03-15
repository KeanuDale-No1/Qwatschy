using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Shared.Models
{
    public class UserDTO
    {
        public Guid ClientID { get; set; }
        public string DisplayName { get; set; }

        public Guid? ChannelId { get; set; }
    }
}
