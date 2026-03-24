using Avalonia;
using Avalonia.Browser;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using VoiceChat.Client;
using VoiceChat.Client.Extensions;
using VoiceChat.Client.Services.SoundPlayer;
using VoiceChat.Client.Services.VoiceService;

internal sealed partial class Program
{
    private static Task Main(string[] args)
    {
        var services = ConfigureServices();
        
        return BuildAvaloniaApp()
            .AfterSetup(_ =>
            {
                App.Services = services;
            })
            .WithInterFont()
            .StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>().WithInterFont();

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        //services.AddSingleton<ISoundPlayer, BrowserSoundPlayer>();
        services.AddSingleton<IVoiceService, NullVoiceService>();
        services.AddCommonServices();
        return services.BuildServiceProvider();
    }
}