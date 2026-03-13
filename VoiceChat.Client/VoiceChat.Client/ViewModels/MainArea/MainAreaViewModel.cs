
using CommunityToolkit.Mvvm.ComponentModel;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea
{
    public partial class MainAreaViewModel : ViewModelBase
    {

        [ObservableProperty] private ChannelSidebarViewModel channelSidebarViewModel;

        public ChatViewModel Chat { get; }

        [ObservableProperty]  private bool isSettingsOpen;
        public string? UrlPathSegment => throw new System.NotImplementedException();

        private readonly AppState appState;

        public MainAreaViewModel(IHttpClientService httpClientService,ChatService chatService,AppState appState, ChannelSidebarViewModel channelSidebarViewModel)
        {
            this.appState = appState;
            this.channelSidebarViewModel = channelSidebarViewModel;
            Chat = new ChatViewModel(chatService, appState);
        }

    }
}
