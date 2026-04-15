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
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Ein Name ist erforderlich")]
    [Length(2,30,ErrorMessage = "Der Name muss 2 bis 20 Zeichen enthalten")]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public string username = "";

    public LoginViewModel(IAppSettingsService appSettingsService, INavigationService navigationService)
    {
        this.appSettingsService = appSettingsService;
        this.navigationService = navigationService;
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    public async Task Save()
    {
        
        appSettingsService.SetUsername(Username);
        await navigationService.NavigateTo<MainAreaViewModel>();
    }
    private bool CanSave() => !HasErrors && !string.IsNullOrWhiteSpace(Username);

}
