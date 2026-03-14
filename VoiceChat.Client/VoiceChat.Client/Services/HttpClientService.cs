using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Services;


public class HttpClientService : IHttpClientService
{
    private readonly TokenService tokenService;

    private HttpClient _client;

    public HttpClientService(TokenService tokenService)
    {
        _client = new HttpClient();
        this.tokenService = tokenService;
    }

    public async Task CreateClient(string serveradress) {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(serveradress);
        
    }


    private void refreshToken()
    {
        if (!string.IsNullOrWhiteSpace(tokenService.ReadToken()))
        {
            _client.DefaultRequestHeaders.Authorization =
                       new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenService.ReadToken());
        }
    }


  

    //public async Task<string> PostAsync<T>(string url, T data)
    //{
    //    var response = await _client.PostAsJsonAsync(url, data, Json.Options);
    //    response.EnsureSuccessStatusCode();
    //    return await response.Content.ReadAsStringAsync();
    //}

    public async Task<T1> PostAsync<T,T1>(string url, T data)
    {
        try
        {
            refreshToken();
            var result = await _client.PostAsync(url, JsonContent.Create(data, options: Json.Options));
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