using System.Collections.Generic;
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
[JsonSerializable(typeof(GetMessagesRequestDTO))]
[JsonSerializable(typeof(GetMessagesResponseDTO))]
[JsonSerializable(typeof(List<ChatMessageDTO>))]
[JsonSerializable(typeof(ChatMessageDTO))]
[JsonSerializable(typeof(ChannelDTO))]
[JsonSerializable(typeof(ConnectChannelRequestDTO))]
[JsonSerializable(typeof(ConnectChannelResponseDTO))]
[JsonSerializable(typeof(CreateChatMessageRequestDTO))]
[JsonSerializable(typeof(CreateChatMessageResponseDTO))]
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