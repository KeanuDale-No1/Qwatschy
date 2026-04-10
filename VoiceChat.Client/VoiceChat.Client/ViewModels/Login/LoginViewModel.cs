using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.Login;


public partial class LoginViewModel : ViewModelBase
{
    private readonly IAppSettingsService appState;

    [ObservableProperty] public string username = "";

    public LoginViewModel(IAppSettingsService appState)
    {
        this.appState = appState;
        Username = appState.AppSetting.UserSettings.Username;
    }
    public LoginViewModel() : this(null!) { }

    [RelayCommand]
    public async Task Save()
    {
        appState.AppSetting.UserSettings.Username = Username;
        appState.SaveAppSettings();
    }
}
