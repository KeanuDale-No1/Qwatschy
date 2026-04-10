using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Client.Services.Navigation;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.Login;


public partial class LoginViewModel : ViewModelBase
{
    private readonly IAppSettingsService appSettingsService;
    private readonly INavigationService navigationService;


    [ObservableProperty]
    public string username = "";

    public LoginViewModel(IAppSettingsService appSettingsService, INavigationService navigationService)
    {
        this.appSettingsService = appSettingsService;
        this.navigationService = navigationService;
    }

    [RelayCommand]
    public async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Username))
            return;
        appSettingsService.SetUsername(Username);
        await navigationService.NavigateTo<MainAreaViewModel>();
    }
}
