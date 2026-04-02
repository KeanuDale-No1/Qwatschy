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
    private readonly VoiceHubClient voiceHubClient;
    //private readonly VoiceChannelViewModel voiceChannelViewModel;

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

    [ObservableProperty] public bool isInVoiceChannel;


    public ChannelSidebarViewModel(ChannelService channelService, Sounds sounds, VoiceHubClient voiceHubClient ) //  VoiceChannelViewModel voiceChannelViewModel)
    {

        this.channelService = channelService;
        this.sounds = sounds;
        //this.voiceChannelViewModel = voiceChannelViewModel;
        this.voiceHubClient = voiceHubClient;
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
        try
        {
            sounds.PlayJoinSound();

            await channelService.JoinChannel(channel);
            await voiceHubClient.LeaveChannelAsync();
            await voiceHubClient.ConnectAsync();

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }

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

    //[RelayCommand]
    //public async Task ToggleVoiceChannel()
    //{
    //    Console.WriteLine("[ChannelSidebar] ToggleVoiceChannelCommand called");
    //    Console.WriteLine($"[ChannelSidebar] SelectedChannel: {SelectedChannel?.Name ?? "null"}");
        
    //    if (SelectedChannel == null)
    //    {
    //        Console.WriteLine("[ChannelSidebar] No channel selected, returning");
    //        return;
    //    }

    //    if (IsInVoiceChannel)
    //    {
    //        Console.WriteLine("[ChannelSidebar] Leaving voice channel");
    //        await voiceChannelViewModel.LeaveVoiceChannel();
    //        IsInVoiceChannel = false;
    //    }
    //    else
    //    {
    //        Console.WriteLine($"[ChannelSidebar] Joining voice channel: {SelectedChannel.Name}");
    //        await channelService.JoinChannel(SelectedChannel);
    //        await voiceChannelViewModel.JoinVoiceChannel(SelectedChannel.Id, SelectedChannel.Name);
    //        IsInVoiceChannel = true;
    //    }
    //}
}