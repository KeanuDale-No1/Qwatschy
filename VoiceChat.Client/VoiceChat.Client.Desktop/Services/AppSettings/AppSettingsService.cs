using System;
using System.IO;
using System.Text.Json;
using VoiceChat.Client.Services.AppSettings;

namespace VoiceChat.Client.Desktop.Services.AppSettings;

internal class AppSettingsService : IAppSettingsService
{
    public bool NewAppSetting { get; private set; } = true;
    public AppSetting AppSetting { get; private set; } 


    private string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), QwatschyConstants.AppName, "settings.dat");
    public AppSettingsService()
    {
    }

    public void InitAppSettings()
    {
        try
        {
            var path = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(filePath))
            {
                AppSetting = new AppSetting();
                return;
            }
            var json = File.ReadAllText(filePath);
            var clientData = JsonSerializer.Deserialize<AppSetting>(json);
            AppSetting = clientData ?? new AppSetting();
            NewAppSetting = false;
        }
        catch (Exception ex)
        {
            
            throw;
        }
    }

    private void SaveAppSettings()
    {
        try
        {
            var json = JsonSerializer.Serialize(AppSetting, AppSettingContext.Default.AppSetting);
            var path = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(filePath, json);
            NewAppSetting = false;
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (IOException)
        {
        }
    }

    public void SetUsername(string Username)
    {
        AppSetting.UserSettings.Username = Username;
        SaveAppSettings();
    }

}
