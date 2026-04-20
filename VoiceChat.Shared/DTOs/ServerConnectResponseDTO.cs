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
    ConnectedUser[] ConnectedUsers);

public record ConnectedUser(
    Guid Id,
    string Username);