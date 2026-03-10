
using ReactiveUI;
using VoiceChat.Client.ViewModels;

namespace VoiceChat.Client.ViewModels
{
    public class MainViewModel : PageViewModelBase
    {
        public ChannelSidebarViewModel LeftSidebar { get; } = new();
        public ChatViewModel Chat { get; } = new();
        public RightSidebarViewModel RightSidebar { get; }

        private bool isSettingsOpen;
        public bool IsSettingsOpen
        {
            get => isSettingsOpen;
            set => this.RaiseAndSetIfChanged(ref isSettingsOpen, value);
        }
        public override bool CanNavigateNext { get => true; protected set => throw new System.NotImplementedException(); }
        public override bool CanNavigatePrevious { get => true; protected set => throw new System.NotImplementedException(); }

        public MainViewModel()
        {
            RightSidebar = new RightSidebarViewModel(this);
        }

    }
}
