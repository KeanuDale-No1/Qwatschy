using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services;
public class ApiService 
{
    private readonly TokenService tokenService;

    private IHttpClientFactory factory;

    public ApiService(TokenService tokenService, IHttpClientFactory _factory)
    {
        factory = _factory;
        this.tokenService = tokenService;
    }

    public async Task<T1> PostAsync<T,T1>(string url, T data)
    {
        try
        {
            var client = factory.CreateClient("ApiClient");
            if (!string.IsNullOrWhiteSpace(tokenService.ReadToken()))
            {
                client.DefaultRequestHeaders.Authorization =
                           new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenService.ReadToken());
            }
        
            var result = await client.PostAsync(url, JsonContent.Create(data, options: Json.Options));
            if (!result.IsSuccessStatusCode)
            {
                var errorContent = await result.Content.ReadAsStringAsync();
                
                throw new HttpRequestException($"Request failed with status code {result.StatusCode}: {errorContent}");
            }
            return await result.Content.ReadFromJsonAsync<T1>(Json.Options) ?? throw new Exception("Failed to deserialize response.");
        }
        catch (Exception)
        {
            throw;
        }
    }

}