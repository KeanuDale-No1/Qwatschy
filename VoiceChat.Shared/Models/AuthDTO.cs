using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Shared.Models;

public record LoginRequestDTO(Guid ClientId, string DisplayName);
public record LoginResponseDTO(Guid UserId, string DisplayName, string AuthToken);

