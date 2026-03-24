using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.Utilitis;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.MainArea;

public partial class ChannelSidebarViewModel : ViewModelBase
{
    private readonly ChannelService channelService;
    private readonly Sounds sounds;
    public ObservableCollection<ChannelDTO> Channels { get; } = new ObservableCollection<ChannelDTO>
            {
                new ChannelDTO() {
                    Id = Guid.NewGuid(),
                    Description = "",
                    Name = "Room1",
                    UnreadCount = 1 },
                new ChannelDTO() {
                    Id = Guid.NewGuid(),
                    Name = "Room 2",
                    UnreadCount = 0,
                    Description = "test"
                }};
    public ObservableCollection<UserDTO> SelectedChannelUsers { get; set; } = new ObservableCollection<UserDTO>();

    [ObservableProperty] public ChannelDTO? selectedChannel;


    [ObservableProperty] public string newChannelName = "";


    public ChannelSidebarViewModel(ChannelService channelService, Sounds sounds)
    {

        this.channelService = channelService;
        this.sounds = sounds;
        Channels = channelService.Channels;
        SelectedChannelUsers = channelService.ChannelUsers;
    }
    public ChannelSidebarViewModel()
    {

        if (!Design.IsDesignMode)
            throw new InvalidOperationException(
                "Parameterloser Konstruktor darf nur im Designer verwendet werden.");

        // Dummy-Daten für Designer
        Channels = new ObservableCollection<ChannelDTO>
            {
                new ChannelDTO() {
                    Id = Guid.NewGuid(),
                    Description = "",
                    Name = "Room1",
                    UnreadCount = 1 },
                new ChannelDTO() {
                    Id = Guid.NewGuid(),
                    Name = "Room 2",
                    UnreadCount = 0,
                    Description = "test"
                }};
    }



    [RelayCommand]
    private async Task JoinChannel(ChannelDTO channel)
    {
        sounds.PlayJoinSound();

        await channelService.JoinChannel(channel);
    }


    [RelayCommand]
    public async Task CreateChannel()
    {

        await channelService.AddChannel(new ChannelDTO() { Id = Guid.NewGuid(), Name = NewChannelName, Description = "" });
        NewChannelName = "";

    }


    [RelayCommand]
    private async Task DeleteChannel(ChannelDTO channel)
    {
        await channelService.DeleteChannel(channel);
    }
    [RelayCommand]
    private async Task SelectChannel(ChannelDTO channel)
    {
        SelectedChannel = channel;
        channelService.SetSelectChannel(channel);
    }

    [RelayCommand]
    private async Task KickUser(UserDTO user)
    {
        if (SelectedChannel != null)
        {
            await channelService.KickUser(SelectedChannel.Id, user.ClientID);
        }
    }

    [RelayCommand]
    private async Task BanUser(UserDTO user)
    {
        if (SelectedChannel != null)
        {
            await channelService.BanUser(SelectedChannel.Id, user.ClientID);
        }
    }
}