namespace VoiceChat.Shared.Models;

public record ServerConnectResponseDTO(
    string Token,
    string ServerName,
    string? ServerImage
);
