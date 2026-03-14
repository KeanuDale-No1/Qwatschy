using System.Text.Json;
using System.Text.Json.Serialization;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client;


[JsonSerializable(typeof(LoginRequestDTO))]
[JsonSerializable(typeof(LoginResponseDTO))]
[JsonSerializable(typeof(CreateChannelRequestDTO))]
[JsonSerializable(typeof(CreateChannelResponseDTO))]
[JsonSerializable(typeof(GetChannelsRequestDTO))]
[JsonSerializable(typeof(GetChannelsResponseDTO))]
[JsonSerializable(typeof(DeleteChannelRequestDTO))]
[JsonSerializable(typeof(DeleteChannelResponseDTO))]
public partial class AppJsonContext : JsonSerializerContext
{ }



public static class Json
{
    public static readonly JsonSerializerOptions Options =
        new JsonSerializerOptions
        {
            TypeInfoResolver = AppJsonContext.Default
        };
}