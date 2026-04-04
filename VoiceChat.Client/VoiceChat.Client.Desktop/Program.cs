using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using System;
using VoiceChat.Client.Desktop.Services.SoundPlayer;
using VoiceChat.Client.Extensions;
using VoiceChat.Client.Services.SoundPlayer;
using VoiceChat.Client.Services.VoiceService;

namespace VoiceChat.Client.Desktop
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            var services = ConfigureServices();

            BuildAvaloniaApp()
                .AfterSetup(_ =>
                {
                    App.Services = services;
                })
                .StartWithClassicDesktopLifetime(args);
        }
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Hier registrierst du deinen Desktop-SoundPlayer
            services.AddSingleton<ISoundPlayer, DesktopSoundPlayer>();
            services.AddSingleton<IVoiceService, Services.VoiceService.DesktopVoiceService>();
            services.AddCommonServices();
            // weitere Services...

            return services.BuildServiceProvider();
        }

    }
}
