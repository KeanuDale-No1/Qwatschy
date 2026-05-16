using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using System;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Client.Services.DialogService;
using VoiceChat.Client.Services.Navigation;
using VoiceChat.Client.Services.ServerViewService;
using VoiceChat.Client.Services.ServerViewServices;
using VoiceChat.Client.Services.TokenProviders;
using VoiceChat.Client.Services.VoiceService;
using VoiceChat.Client.Utilitis;
using VoiceChat.Client.ViewModels;
using VoiceChat.Client.ViewModels.Dialog;
using VoiceChat.Client.ViewModels.Login;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Client.ViewModels.MainArea.Componets;

namespace VoiceChat.Client.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        //Services
        services.AddSingleton<IAppSettingsService, AppSettingsService>();
        services.AddSingleton<VoiceHubClient>();

        services.AddHttpClient<ITokenProvider, TokenProvider>();
        services.AddSingleton<ClientHub>();

        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<IServerViewService, ServerViewService>();
        #region DialogService
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<DialogServiceViewSetterService>();
        services.AddSingleton<DialogViewModel>();
        #endregion


        services.AddSingleton<StateService>();
      
       
        services.AddSingleton<VoiceChatService>();
        services.AddTransient<Sounds>();


        //ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainAreaViewModel>();
        services.AddTransient<ChannelSidebarViewModel>();
        services.AddTransient<ChatViewModel>();
        services.AddTransient<ServerInformationViewModel>();
        services.AddTransient<ServerConnectionsViewModel>();
        services.AddTransient<ActionbarViewModel>();
        services.AddTransient<AddServerDialogViewModel>();
        services.AddTransient<AddChannelDialogViewModel>();

    }
}