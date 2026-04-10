using CommunityToolkit.Mvvm.ComponentModel;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea;

public partial class MainAreaViewModel : ViewModelBase
{
    public MainAreaViewModel(ChannelService channelService)
    {
        _ = channelService.LoadChannelInitial();
    }
}