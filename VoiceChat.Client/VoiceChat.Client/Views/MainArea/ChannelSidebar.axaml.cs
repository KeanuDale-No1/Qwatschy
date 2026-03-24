using Avalonia;
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

    private void OnUserClick(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed && 
            sender is Border border && 
            border.DataContext is UserDTO user)
        {
            var viewModel = this.DataContext as ChannelSidebarViewModel;
            if (viewModel == null) return;

            var menu = new ContextMenu();

            var kickItem = new MenuItem { Header = "Kick" };
            kickItem.Click += (_, _) => viewModel.KickUserCommand.Execute(user);
            menu.Items.Add(kickItem);

            var banItem = new MenuItem { Header = "Ban" };
            banItem.Click += (_, _) => viewModel.BanUserCommand.Execute(user);
            menu.Items.Add(banItem);

            menu.Open(border);
        }
    }
}
