using System;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Client.Services.AppSettings;

namespace VoiceChat.Client.Browser.Services.AppSettings;

[SupportedOSPlatform("browser")]
internal partial class LocalStorageService : IStorageService
{
    private const string StorageKey = QwatschyConstants.AppName + ".Settings";

    [JSImport("getItem", "localStorage")]
    internal static partial string? GetItemJs(string key);

    [JSImport("setItem", "localStorage")]
    internal static partial void SetItemJs(string key, string value);

    public string? Load()
    {
        var encoded = GetItemJs(StorageKey);
        if (string.IsNullOrEmpty(encoded))
            return null;

        try
        {
            var json = FromBase64(encoded);
            return json;
        }
        catch
        {
            return null;
        }
    }

    public Task SaveAsync(string json)
    {
        var encoded = ToBase64(json);
        SetItemJs(StorageKey, encoded);
        return Task.CompletedTask;
    }

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