using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
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
        try
        {
            _client.BaseAddress = new Uri(_appState.ServerAdress);
            if (_appState?.ClientData?.JwtToken != null && !string.IsNullOrWhiteSpace(_appState?.ClientData?.JwtToken))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _appState.ClientData.JwtToken);
            }
        }
        catch (Exception ex)
        {

            throw;
        }
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
        return await _client.GetFromJsonAsync<T>(url, Json.Options);
    }

    public async Task<string> PostAsync<T>(string url, T data)
    {
        ApplyToken();
        var response = await _client.PostAsJsonAsync(url, data, Json.Options);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<T1> PostAsync<T,T1>(string url, T data)
    {
        try
        {
            ApplyToken();

            var result = await _client.PostAsJsonAsync<T>(url, data, Json.Options);
            return await result.Content.ReadFromJsonAsync<T1>();
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}