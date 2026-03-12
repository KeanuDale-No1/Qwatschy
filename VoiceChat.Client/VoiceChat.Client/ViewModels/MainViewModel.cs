

using CommunityToolkit.Mvvm.ComponentModel;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Client.ViewModels.Login;

namespace VoiceChat.Client.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    [ObservableProperty] private ViewModelBase _currentViewModel;


    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        ((NavigationService)navigationService).SetMainViewModel(this);
        navigationService.NavigateTo<LoginViewModel>();
    }

    public MainViewModel() : this(null!) { }

}
