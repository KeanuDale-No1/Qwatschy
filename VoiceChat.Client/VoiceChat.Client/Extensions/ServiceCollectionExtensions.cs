using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using System;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.Services.VoiceService;
using VoiceChat.Client.Utilitis;
using VoiceChat.Client.ViewModels;
using VoiceChat.Client.ViewModels.Login;
using VoiceChat.Client.ViewModels.MainArea;

namespace VoiceChat.Client.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        //Services
        services.AddSingleton<TokenService>();
        services.AddSingleton<AppState>();
        services.AddSingleton<ServiceHubClient>();
        services.AddSingleton<ConnectionService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<StateService>();
        services.AddHttpClient("ApiClient",(sp, client) =>
        {
            var settings = sp.GetRequiredService<StateService>();
            if (!string.IsNullOrWhiteSpace(settings.ServerAddress))
                client.BaseAddress = new Uri(settings.ServerAddress);
        });
        services.AddTransient<ApiService>();
        services.AddSingleton<ChannelService>();
        services.AddSingleton<VoiceChatService>();
        services.AddSingleton<IApplicationLifetime>(sp => Application.Current!.ApplicationLifetime!);
        services.AddTransient<Sounds>();


        //ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainAreaViewModel>();
        services.AddTransient<ChannelSidebarViewModel>();
        services.AddTransient<ChatViewModel>();

    }
}