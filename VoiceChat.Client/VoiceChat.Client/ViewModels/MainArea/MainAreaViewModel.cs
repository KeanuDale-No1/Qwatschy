
using CommunityToolkit.Mvvm.ComponentModel;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea
{
    public partial class MainAreaViewModel : ViewModelBase
    {

        [ObservableProperty] private ChannelSidebarViewModel channelSidebarViewModel;

        public ChatViewModel Chat { get; }


        public MainAreaViewModel(IHttpClientService httpClientService, ChannelSidebarViewModel channelSidebarViewModel)
        {
            this.channelSidebarViewModel = channelSidebarViewModel;
        }

    }
}
