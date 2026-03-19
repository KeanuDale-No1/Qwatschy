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

        //var collection = new ServiceCollection();
        //collection.AddCommonServices();
        //var services = collection.BuildServiceProvider();
        if (!Design.IsDesignMode)
        {
            var appState = Services.GetRequiredService<AppState>();
            appState.ApplicationLifetime = ApplicationLifetime;
        }

        var vm = Design.IsDesignMode ? new MainViewModel() : Services.GetRequiredService<MainViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

}