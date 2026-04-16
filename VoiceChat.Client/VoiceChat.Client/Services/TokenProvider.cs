using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Client.Services;

public class TokenProvider : ITokenProvider
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public TokenProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = Json.Options;
    }

    public async Task<ServerConnectResponseDTO?> GetServerInfoAsync(string serverAddress, Guid clientId, string? displayName)
    {
        try
        {
            var url = serverAddress.TrimEnd('/') + "/api/server/connect";
            var request = new LoginRequestDTO(clientId, displayName);
            
            var response = await _httpClient.PostAsJsonAsync(url, request, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ServerConnectResponseDTO>(_jsonOptions);
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }
}
