
using CommunityToolkit.Mvvm.ComponentModel;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea
{
    public partial class MainAreaViewModel : ViewModelBase
    {
        [ObservableProperty] public ChannelSidebarViewModel channelSidebarViewModel;
        public ChatViewModel Chat { get; }
        public RightSidebarViewModel RightSidebar { get; }

        [ObservableProperty]  private bool isSettingsOpen;
        public string? UrlPathSegment => throw new System.NotImplementedException();


        public MainAreaViewModel(IHttpClientService httpClientService,ChatService chatService)
        {
            channelSidebarViewModel = new ChannelSidebarViewModel(httpClientService);

            RightSidebar = new RightSidebarViewModel(this);
            Chat = new ChatViewModel(chatService);
        }

    }
}
