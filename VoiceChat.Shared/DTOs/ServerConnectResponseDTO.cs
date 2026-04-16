namespace VoiceChat.Shared.DTOs;

public record ServerConnectResponseDTO(
    string Token,
    string ServerName,
    string? ServerImage,
    string? Description
);
