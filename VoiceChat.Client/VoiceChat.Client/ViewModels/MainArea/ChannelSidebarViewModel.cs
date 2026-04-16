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
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Client.ViewModels.MainArea;

public partial class ChannelSidebarViewModel : ViewModelBase
{
    private readonly Sounds sounds;
    private readonly VoiceHubClient voiceHubClient;
    //private readonly VoiceChannelViewModel voiceChannelViewModel;

   


 

    [ObservableProperty] public bool isInVoiceChannel;


    public ChannelSidebarViewModel( Sounds sounds, VoiceHubClient voiceHubClient ) 
    {

        this.sounds = sounds;
        //this.voiceChannelViewModel = voiceChannelViewModel;
        this.voiceHubClient = voiceHubClient;
    }
    public ChannelSidebarViewModel()
    {

        if (!Design.IsDesignMode)
            throw new InvalidOperationException(
                "Parameterloser Konstruktor darf nur im Designer verwendet werden.");

    }



    [RelayCommand]
    private async Task JoinChannel()
    {
        try
        {
            sounds.PlayJoinSound();

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


    }


    [RelayCommand]
    private async Task DeleteChannel()
    {
    }
    [RelayCommand]
    private async Task SelectChannel()
    {
    }

    [RelayCommand]
    private async Task KickUser()
    {
    }

    [RelayCommand]
    private async Task BanUser()
    {
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