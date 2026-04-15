using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using VoiceChat.Client.Services.AppSettings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VoiceChat.Client.Browser.Services.AppSettings;

[SupportedOSPlatform("browser")]
internal partial class AppSettingsService : IAppSettingsService
{
    #region JS Interop

    [JSImport("getItem", "localStorage")]
    internal static partial string? GetItemJs(string key);

    [JSImport("setItem", "localStorage")]
    internal static partial void SetItemJs(string key, string value);
    #endregion




    private const string StorageKey = QwatschyConstants.AppName + ".Settings";

    public AppSetting AppSetting { get; private set; } = new();

    public bool NewAppSetting { get; private set; } = true;

    public AppSettingsService()
    {
    }

    public void InitAppSettings()
    {
        var encoded = GetItemJs(StorageKey);
        if (string.IsNullOrEmpty(encoded))
        {
            AppSetting = new AppSetting();
            return;
        }

        try
        {
            var json = FromBase64(encoded);
            var settings = JsonSerializer.Deserialize(json, AppSettingContext.Default.AppSetting);
            AppSetting = settings ?? new AppSetting();
            NewAppSetting = false;
        }
        catch
        {
            AppSetting = new AppSetting();
        }
    }

    private void SaveAppSettings()
    {
        var json = JsonSerializer.Serialize(AppSetting, AppSettingContext.Default.AppSetting);
        var encoded = ToBase64(json);
        SetItemJs(StorageKey, encoded);
        NewAppSetting = false;
    }
    public void SetUsername(string Username)
    {
        AppSetting.UserSettings.Username = Username;
        SaveAppSettings();
    }
    public void AddServer(Guid serverId, string ServerAdress)
    {
        AppSetting.Servers.ServerAddresses.Add(new ServerSettings { ServerId = serverId, ServerAddress = ServerAdress });
        SaveAppSettings();
    }

    public void RemoveServer(Guid serverId)
    {
        var server = AppSetting.Servers.ServerAddresses.FirstOrDefault(x => x.ServerId == serverId);
        if (server != null)
        {
            AppSetting.Servers.ServerAddresses.Remove(server);
            SaveAppSettings();
        }
    }

    /// ------------ Helper Methods ------------
    private static string ToBase64(string plainText)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    private static string FromBase64(string encoded)
    {
        var bytes = Convert.FromBase64String(encoded);
        return Encoding.UTF8.GetString(bytes);
    }

    
}
