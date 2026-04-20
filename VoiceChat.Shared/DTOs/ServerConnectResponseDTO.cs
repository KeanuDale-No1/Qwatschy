namespace VoiceChat.Shared.DTOs;

public record ServerConnectResponseDTO(
    string Token,
    string ServerName,
    string? ServerImage,
    string? Description,
    ChannelDTO[] Channels
);


public record ChannelDTO(
    Guid Id,
    string Name,
    string? Description,
    ConnectedUserDTO[] ConnectedUsers);

public record ConnectedUserDTO(
    Guid Id,
    string Username);