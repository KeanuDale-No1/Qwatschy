using VoiceChat.Client.Services.AppSettings;

namespace VoiceChat.Client.Services;

public static class ServiceLocator
{
    public static IAppSettingsService? AppSettingsService { get; set; }
}
