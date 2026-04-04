using Avalonia;
using Avalonia.Browser;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using VoiceChat.Client;
using VoiceChat.Client.Extensions;
using VoiceChat.Client.Services.SoundPlayer;
using VoiceChat.Client.Services.VoiceService;
using VoiceChat.Client.Browser.Services;

internal sealed partial class Program
{
    private static async Task Main(string[] args)
    {
        var services = ConfigureServices();
        
        await BuildAvaloniaApp()
            .AfterSetup(_ =>
            {
                App.Services = services;
            })
            .WithInterFont()
            .StartBrowserAppAsync("out");
        
        await JSHost.ImportAsync("audioService", "../js/audioService.js");
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>().WithInterFont();

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IVoiceService, BrowserVoiceService>();
        services.AddCommonServices();
        return services.BuildServiceProvider();
    }
}