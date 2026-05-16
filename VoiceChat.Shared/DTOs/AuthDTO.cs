using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Shared.DTOs;

public record LoginRequestDTO(Guid ClientId, string DisplayName);

