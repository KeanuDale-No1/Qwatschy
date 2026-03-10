using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels;
using VoiceChat.Client.Views;

namespace VoiceChat.Client
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {

            var collection = new ServiceCollection();
            collection.AddCommonServices();
            var services = collection.BuildServiceProvider();

            var appState = services.GetRequiredService<AppState>();
            appState.RestoreClientData();

            var vm = services.GetRequiredService<MainWindowViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                //singleViewPlatform.MainView = new MainView
                //{
                //    DataContext = vm
                //};
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}