using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services;

public interface IHttpClientService
{
    Task<string> GetAsync(string url);
    Task<T?> GetJsonAsync<T>(string url);
    Task<string> PostAsync<T>(string url, T data);
}
