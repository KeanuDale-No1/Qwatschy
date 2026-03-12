using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using VoiceChat.Client.ViewModels.MainArea;

namespace VoiceChat.Client.Views.MainArea;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += (_, __) =>
        {
            if (DataContext is ChatViewModel vm)
            {
                vm.MessageAdded += ScrollToBottom;
            }
        };

    }

    private void ScrollToBottom()
    {
        Dispatcher.UIThread.Post(() =>
        {
            ChatScrollViewer.ScrollToEnd();
        }, DispatcherPriority.Background);
    }
}