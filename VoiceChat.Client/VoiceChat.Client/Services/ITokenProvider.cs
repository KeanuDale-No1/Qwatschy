using System;
using System.Threading.Tasks;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Services;

public interface ITokenProvider
{
    Task<ServerConnectResponseDTO?> GetServerInfoAsync(string serverAddress, Guid clientId, string? displayName);
}
