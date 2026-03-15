using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.Utilitis;
using VoiceChat.Client.ViewModels;
using VoiceChat.Client.ViewModels.Login;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Client.Views;

namespace VoiceChat.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            //Services
            collection.AddSingleton<TokenService>();
            collection.AddSingleton<AppState>();
            collection.AddSingleton<ServiceHub>();
            collection.AddSingleton<ConnectionService>();
            collection.AddSingleton<INavigationService, NavigationService>();
            collection.AddSingleton<StatusService>();
            collection.AddSingleton<IHttpClientService, HttpClientService>();
            //collection.AddSingleton<ChatService>();
            collection.AddSingleton<ChannelService>();
            collection.AddSingleton<IApplicationLifetime>(sp => Application.Current!.ApplicationLifetime!);
            collection.AddTransient<Sounds>();


            //ViewModels
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<LoginViewModel>();
            collection.AddTransient<MainAreaViewModel>();
            collection.AddTransient<ChannelSidebarViewModel>();
            collection.AddTransient<StatusBarViewModel>();

        }
    }
}
