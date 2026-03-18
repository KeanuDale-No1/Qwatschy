using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.Utilitis;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.MainArea;

public partial class ChannelSidebarViewModel : ViewModelBase
{
    private readonly StatusService statusService;
    private readonly ChannelService channelService;
    private readonly Sounds sounds;
    public ObservableCollection<ChannelDTO> Channels { get; }
    public ObservableCollection<UserDTO> SelectedChannelUsers { get; set; } = new ObservableCollection<UserDTO>();

    [ObservableProperty] public ChannelDTO? selectedChannel;


    [ObservableProperty] public string newChannelName = "";


    public ChannelSidebarViewModel(StatusService statusService,  ChannelService channelService, Sounds sounds)
    {
        this.statusService = statusService;
        this.channelService = channelService;
        this.sounds = sounds;
        Channels = channelService.Channels;
        SelectedChannelUsers = channelService.ChannelUsers;
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
        try
        {
            await channelService.AddChannel(new ChannelDTO() { Id = Guid.NewGuid(), Name = NewChannelName, Description = "" });
            NewChannelName = "";
        }
        catch (Exception ex)
        {
            statusService.AddReport($"Channel konnte nicht erstellt werden:{ex.Message}");
        }
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
}