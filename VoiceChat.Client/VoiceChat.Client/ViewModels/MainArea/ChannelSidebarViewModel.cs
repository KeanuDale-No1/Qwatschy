using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Hubs;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.MainArea;

public partial class ChannelSidebarViewModel : ViewModelBase
{
    private readonly StatusService statusService;
    private readonly ChannelService channelService;

    public ObservableCollection<ChannelDTO> Channels { get; set; } = new ObservableCollection<ChannelDTO>();
    [ObservableProperty] public string newChannelName = "";



    public ChannelSidebarViewModel(StatusService statusService,  ChannelService channelService)
    {
        this.statusService = statusService;
        this.channelService = channelService;
    }


    [RelayCommand]
    private async Task JoinChannel(ChannelDTO channel)
    {
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
        //var response = await httpClientService.PostAsync<DeleteChannelRequestDTO, DeleteChannelResponseDTO>("api/DeleteChannel", new DeleteChannelRequestDTO(channel.Id));
        //if (response == null || response.isDelete == false)
        //    throw new Exception("Channel konnte nicht gelöscht werden");
        //Channels.Remove(channel);
    }

    private async Task EditChannel()
    {
    }

    private async Task LeaveChannel()
    {

    }

    private async Task LoadOnlineUsers()
    {
        // Hier würden Sie die Online-Benutzer von Ihrem Server laden, z.B.:
        // var users = await httpClientService.GetAsync<List<UserModel>>("api/online-users");
        // OnlineUsers = new ObservableCollection<UserModel>(users);
    }




}