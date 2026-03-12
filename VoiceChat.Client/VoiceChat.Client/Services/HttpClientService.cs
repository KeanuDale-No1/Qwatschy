using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services;


public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _client;
    private readonly AppState _appState;

    public HttpClientService(AppState appState)
    {
        _client = new HttpClient();
        _appState = appState;
    }

    private void ApplyToken()
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _appState.ClientData.JwtToken);
    }

    public async Task<string> GetAsync(string url)
    {
        ApplyToken();
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<T?> GetJsonAsync<T>(string url)
    {
        ApplyToken();
        return await _client.GetFromJsonAsync<T>(url);
    }

    public async Task<string> PostAsync<T>(string url, T data)
    {
        ApplyToken();
        var response = await _client.PostAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}