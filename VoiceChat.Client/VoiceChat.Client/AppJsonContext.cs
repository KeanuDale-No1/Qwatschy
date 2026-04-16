using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Client;

[JsonSerializable(typeof(LoginRequestDTO))]
[JsonSerializable(typeof(ServerConnectResponseDTO))]
public partial class AppJsonContext : JsonSerializerContext
{ }



public static class Json
{
    public static readonly JsonSerializerOptions Options =
        new JsonSerializerOptions
        {
            TypeInfoResolver = AppJsonContext.Default,
            PropertyNameCaseInsensitive = true
        };
}