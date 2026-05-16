using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using VoiceChat.Client.ViewModels;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.Services.Navigation;

internal class NavigationService : INavigationService
{
    private MainViewModel? _mainWindowViewModel;
    private readonly IServiceProvider _serviceProperty;

    public NavigationService(IServiceProvider serviceProperty)
    {
        _serviceProperty = serviceProperty;
    }

    public Task NavigateTo<T>() where T : ViewModelBase
    {
        if (_mainWindowViewModel == null)
            return Task.CompletedTask;

        var vm = _serviceProperty.GetRequiredService<T>();
        _mainWindowViewModel.CurrentViewModel = vm;
        return Task.CompletedTask;
    }

    public void SetMainViewModel(MainViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }
}
