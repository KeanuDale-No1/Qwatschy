using CommunityToolkit.Mvvm.ComponentModel;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea;

public partial class MainAreaViewModel : ViewModelBase
{

    [ObservableProperty] private ChannelSidebarViewModel channelSidebarViewModel;

    [ObservableProperty] public ChatViewModel chatViewModel;

    [ObservableProperty] public ServerInformationViewModel serverInformationViewModel;


    public MainAreaViewModel(ChannelSidebarViewModel channelSidebarViewModel, ServerInformationViewModel serverInformationViewModel, ChatViewModel chatViewModel, ChannelService channelService)
    {
        this.channelSidebarViewModel = channelSidebarViewModel;
        this.serverInformationViewModel = serverInformationViewModel;
        this.chatViewModel = chatViewModel;
        _ = channelService.LoadChannelInitial();
    }

}
