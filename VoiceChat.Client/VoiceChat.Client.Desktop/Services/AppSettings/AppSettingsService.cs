using System;
using System.IO;
using System.Text.Json;
using VoiceChat.Client.Services.AppSettings;

namespace VoiceChat.Client.Desktop.Services.AppSettings;

internal class AppSettingsService : IAppSettingsService
{
    public AppSetting AppSetting { get; set; } 


    private string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), QwatschyConstants.AppName, "settings.dat");
    public AppSettingsService()
    {
    }

    public  void InitAppSettings()
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
        }
        catch (Exception ex)
        {
            
            throw;
        }
    }

    public void SaveAppSettings()
    {
        try
        {
            var json = JsonSerializer.Serialize(AppSetting, AppSettingContext.Default.AppSetting);
            var path = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(filePath, json);
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (IOException)
        {
        }
    }
}
