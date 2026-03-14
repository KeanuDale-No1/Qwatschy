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
    private HttpClient _client;
    private readonly AppState _appState;

    public HttpClientService(AppState appState)
    {
        _appState = appState;
        _client = new HttpClient();
    }

    public async Task CreateClient(string serveradress, string? token) {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(serveradress);
        if (!string.IsNullOrWhiteSpace(token)) {
            _client.DefaultRequestHeaders.Authorization =
                       new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }


    public async Task<string> GetAsync(string url)
    {
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<T?> GetJsonAsync<T>(string url)
    {
        return await _client.GetFromJsonAsync<T>(url, Json.Options);
    }

    public async Task<string> PostAsync<T>(string url, T data)
    {
        var response = await _client.PostAsJsonAsync(url, data, Json.Options);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<T1> PostAsync<T,T1>(string url, T data)
    {
        try
        {

            var result1 = await _client.PostAsync(url, JsonContent.Create(data, options: Json.Options));
            if (!result1.IsSuccessStatusCode)
            {
                var errorContent = await result1.Content.ReadAsStringAsync();
                
                throw new HttpRequestException($"Request failed with status code {result1.StatusCode}: {errorContent}");
            }
            return await result1.Content.ReadFromJsonAsync<T1>(Json.Options) ?? throw new Exception("Failed to deserialize response.");


            var result = await _client.PostAsJsonAsync<T>(url, data, Json.Options);
            return await result.Content.ReadFromJsonAsync<T1>();
        }
        catch (Exception)
        {
            throw;
        }
    }

}