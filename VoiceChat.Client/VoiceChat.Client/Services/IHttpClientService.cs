using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services;

public interface IHttpClientService
{
    Task CreateClient(string serveradress, string? token);
    Task<string> GetAsync(string url);
    Task<T?> GetJsonAsync<T>(string url);
    Task<string> PostAsync<T>(string url, T data);
    Task<T1> PostAsync<T,T1>(string url, T data);

}
