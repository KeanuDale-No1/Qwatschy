using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Shared.Models
{
    public record ConnectChannelRequestDTO(Guid UserId, Guid ChannelId);

    public record class ConnectChannelResponseDTO(Guid UserId, string Username, Guid ChannelId);
}
