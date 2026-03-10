using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels;

namespace VoiceChat.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<AppState>();
            collection.AddTransient<MainWindowViewModel>();
        }
    }
}
