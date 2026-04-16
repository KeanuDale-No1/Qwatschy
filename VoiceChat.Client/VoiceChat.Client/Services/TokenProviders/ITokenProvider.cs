using System;
using System.Threading.Tasks;
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Client.Services.TokenProviders;

public interface ITokenProvider
{
    Task<ServerConnectResponseDTO?> GetServerInfoAsync(string serverAddress, Guid clientId, string? displayName);
}
