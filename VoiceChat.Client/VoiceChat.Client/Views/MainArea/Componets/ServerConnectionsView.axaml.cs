using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VoiceChat.Client.ViewModels.MainArea.Componets;

namespace VoiceChat.Client.Views.MainArea.Componets;

public partial class ServerConnectionsView : UserControl
{
    public ServerConnectionsView()
    {
        InitializeComponent();
        AttachedToVisualTree += OnAttachedToVisualTree;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (DataContext is ServerConnectionsViewModel vm)
        {
            vm.Initialize();
        }
    }
}