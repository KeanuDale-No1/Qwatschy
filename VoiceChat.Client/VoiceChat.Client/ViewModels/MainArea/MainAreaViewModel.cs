using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea;

public partial class MainAreaViewModel : ViewModelBase
{
    [ObservableProperty] bool isBusy = false;
    private readonly ClientHub clientHub;
    public MainAreaViewModel(ClientHub clientHub)
    {
        this.clientHub = clientHub;
    }

    
    public async Task Init()
    {
        IsBusy = true;
        await clientHub.ConnectAllAsync();
        IsBusy = false;
    }


}