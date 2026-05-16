using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services.AppSettings;

public class AppSettingsService(IStorageService storageService) : IAppSettingsService
{
    private readonly IStorageService _storageService = storageService;

    public AppSetting AppSetting { get; private set; } = new();
    public bool NewAppSetting { get; private set; } = true;

    public void InitAppSettings()
    {
        var json = _storageService.Load();
        if (string.IsNullOrEmpty(json))
        {
            AppSetting = new AppSetting();
            return;
        }

        try
        {
            var settings = JsonSerializer.Deserialize(json, AppSettingContext.Default.AppSetting);
            AppSetting = settings ?? new AppSetting();
            NewAppSetting = false;
        }
        catch
        {
            AppSetting = new AppSetting();
        }
    }

    private async Task SaveAppSettingsAsync()
    {
        var json = JsonSerializer.Serialize(AppSetting, AppSettingContext.Default.AppSetting);
        await _storageService.SaveAsync(json);
        NewAppSetting = false;
    }

    public void SetUsername(string Username)
    {
        AppSetting.UserSettings.Username = Username;
        _ = SaveAppSettingsAsync();
    }

    public void AddServer(Guid serverId, string serverAdress)
    {
        AppSetting.Servers.ServerAddresses.Add(new ServerSettings { ServerId = serverId, ServerAddress = serverAdress });
        _ = SaveAppSettingsAsync();
    }

    public void RemoveServer(Guid serverId)
    {
        var server = AppSetting.Servers.ServerAddresses.FirstOrDefault(x => x.ServerId == serverId);
        if (server != null)
        {
            AppSetting.Servers.ServerAddresses.Remove(server);
            _ = SaveAppSettingsAsync();
        }
    }
}