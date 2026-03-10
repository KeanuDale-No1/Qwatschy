using System.Collections.ObjectModel;

namespace VoiceChat.Client.ViewModels
{
    public class ChannelSidebarViewModel
    {
        public ObservableCollection<string> Channels { get; } = new()
    {
        "#general",
        "#random",
        "#dev",
    };

        public ObservableCollection<string> OnlineUsers { get; } = new()
    {
        "Alice",
        "Bob",
        "Charlie",
    };
    }
}