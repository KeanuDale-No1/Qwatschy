
using CommunityToolkit.Mvvm.ComponentModel;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea
{
    public partial class MainAreaViewModel : ViewModelBase
    {

        [ObservableProperty] private ChannelSidebarViewModel channelSidebarViewModel;

        [ObservableProperty] public ChatViewModel chatViewModel;


        public MainAreaViewModel(ApiService httpClientService, ChannelSidebarViewModel channelSidebarViewModel, ChatViewModel chatViewModel)
        {
            this.channelSidebarViewModel = channelSidebarViewModel;
            this.chatViewModel = chatViewModel;
        }

    }
}
