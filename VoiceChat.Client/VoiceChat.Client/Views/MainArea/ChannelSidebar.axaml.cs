using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using System;
using VoiceChat.Client.ViewModels.MainArea;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.Views.MainArea;

public partial class ChannelSidebarView : UserControl
{
    public ChannelSidebarView()
    {
        InitializeComponent();
    }

    private void OnChannelClick(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is ChannelDTO channel)
        {
            var viewModel = this.DataContext as ChannelSidebarViewModel;
            viewModel?.SelectChannelCommand.Execute(channel);
        }
    }
}
