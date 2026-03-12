using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Client.Services;
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
            collection.AddSingleton<AppState>();
            collection.AddSingleton<INavigationService, NavigationService>();
            collection.AddSingleton<IHttpClientService, HttpClientService>();
            collection.AddSingleton<ChatService>();
            //ViewModels
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<LoginViewModel>();
            collection.AddTransient<MainAreaViewModel>();
            collection.AddTransient<ChannelSidebarViewModel>();

        }
    }
}
