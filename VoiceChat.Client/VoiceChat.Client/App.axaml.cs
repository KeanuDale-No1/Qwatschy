using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using VoiceChat.Client.Extensions;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels;
using VoiceChat.Client.Views;

namespace VoiceChat.Client;

public partial class App : Application
{
    public static IServiceProvider Services { get; set; } = default!;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (Services == null)
        {
            base.OnFrameworkInitializationCompleted();
            return;
        }
        Console.WriteLine(ApplicationLifetime);
        if (!Design.IsDesignMode)
        {
            try
            {
                var appState = Services.GetRequiredService<AppState>();
                appState.ApplicationLifetime = ApplicationLifetime;
            }
            catch { }
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = Services.GetRequiredService<MainViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var vm = Design.IsDesignMode ? new MainViewModel() : Services.GetRequiredService<MainViewModel>();
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

}