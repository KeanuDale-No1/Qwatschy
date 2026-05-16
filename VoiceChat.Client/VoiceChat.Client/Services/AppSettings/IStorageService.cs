using System.Threading.Tasks;

namespace VoiceChat.Client.Services.AppSettings;

public interface IStorageService
{
    string? Load();
    Task SaveAsync(string json);
}