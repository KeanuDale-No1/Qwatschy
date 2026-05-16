

using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using VoiceChat.Client.Services.AppSettings;
using VoiceChat.Client.Services.Navigation;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Client.ViewModels.Login;
using VoiceChat.Client.ViewModels.MainArea;

namespace VoiceChat.Client.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    [ObservableProperty] private ViewModelBase _currentViewModel;


    public MainViewModel(INavigationService navigationService, IAppSettingsService appSettingsService)
    {
        if (!Design.IsDesignMode)
        {
            ((NavigationService)navigationService).SetMainViewModel(this);
            if (appSettingsService.NewAppSetting)
                navigationService.NavigateTo<LoginViewModel>();
            else
                navigationService.NavigateTo<MainAreaViewModel>();

        }
    }


}
