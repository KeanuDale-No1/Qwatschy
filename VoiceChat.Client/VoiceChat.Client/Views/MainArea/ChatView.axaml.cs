using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using VoiceChat.Client.ViewModels.MainArea;

namespace VoiceChat.Client.Views.MainArea;

public partial class ChatView : UserControl
{
    private bool _userScrolledUp = false;
    private double _lastScrollOffset = 0;

    public ChatView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += (_, __) =>
        {
            if (DataContext is ChatViewModel vm)
            {
                vm.MessageAdded += OnMessageAdded;
                vm.ScrollToBottom += OnScrollToBottom;
            }
        };
    }

    private void OnMessageAdded()
    {
        Dispatcher.UIThread.Post(() =>
        {
            ChatScrollViewerMessages.ScrollToEnd();
            _userScrolledUp = false;
        }, DispatcherPriority.Loaded);
    }

    private void OnScrollToBottom()
    {
        Dispatcher.UIThread.Post(() =>
        {
            ChatScrollViewerMessages.ScrollToEnd();
            _userScrolledUp = false;
        }, DispatcherPriority.Loaded);
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (DataContext is not ChatViewModel vm) return;

        var currentOffset = ChatScrollViewerMessages.Offset.Y;
        var maxOffset = ChatScrollViewerMessages.Extent.Height - ChatScrollViewerMessages.Viewport.Height;

        if (maxOffset <= 0) return;

        if (currentOffset <= 10)
        {
            _userScrolledUp = false;
            vm.LoadMoreMessagesCommand.Execute(null);
        }
        else if (currentOffset >= maxOffset - 50)
        {
            _userScrolledUp = false;
        }
        else
        {
            _userScrolledUp = true;
        }

        _lastScrollOffset = currentOffset;
    }
}
