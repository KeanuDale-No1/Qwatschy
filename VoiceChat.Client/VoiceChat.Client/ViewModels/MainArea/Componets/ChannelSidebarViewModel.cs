using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Models;
using VoiceChat.Client.Services.DialogService;
using VoiceChat.Client.Services.ServerViewService;
using VoiceChat.Client.Utilitis;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;

public partial class ChannelSidebarViewModel : ViewModelBase
{
    private readonly VoiceHubClient voiceHubClient;
    private readonly IDialogService dialogService;
    private readonly ClientHub clientHub;
    public IServerViewService ServerViewService { get; }

    
    [ObservableProperty] public bool isInVoiceChannel;


    public ChannelSidebarViewModel( IServerViewService serverViewService, ClientHub clientHub,
                                    VoiceHubClient voiceHubClient, IDialogService dialogService ) 
    {

        //this.voiceChannelViewModel = voiceChannelViewModel;
        ServerViewService = serverViewService;
        this.clientHub = clientHub;
        this.voiceHubClient = voiceHubClient;
        this.dialogService = dialogService;
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
            //sounds.PlayJoinSound();

            await voiceHubClient.LeaveChannelAsync();
            await voiceHubClient.ConnectAsync();
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }

    }

    [RelayCommand]
    public async Task AddChannel()
    {
        var result = await dialogService.ShowDialog<AddChannelDialogViewModel>();
        if (result.IsCanceled)
            return;

        if (result.Data is ValueTuple<string, string?> data)
        {
            var (channelName, description) = data;
            var channelInfo = new ChannelInfo
            {
                Id = Guid.NewGuid(),
                Name = channelName,
                Desciption = description
            };
            await clientHub.AddChannel(ServerViewService.OpenServerInfo.ServerId, channelInfo);
        }
    }


    [RelayCommand]
    private async Task DeleteChannel(ChannelInfo channelInfo)
    {

        if (channelInfo == null)
            return;
        //TODO Dialog abfrage
        await clientHub.DeleteChannel(ServerViewService.OpenServerInfo.ServerId, channelInfo);
    }

    [RelayCommand]
    private async Task SelectChannel(ChannelInfo channelInfo)
    {
        ServerViewService.UpdateChannelInfo(channelInfo);
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