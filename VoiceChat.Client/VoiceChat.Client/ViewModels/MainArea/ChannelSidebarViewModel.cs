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
        channelService.Users.CollectionChanged += async (s, e) =>
        {
            if (SelectedChannel != null)
            {
                await getSelectedChannelUsers();
            }
        };
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
        await getSelectedChannelUsers();
    }

    private async Task getSelectedChannelUsers()
    {
        statusService.AddReport("getSelectedChannelUsers");
       SelectedChannelUsers.Clear();
       foreach (var item in channelService.Users.Where(u => u.ChannelId == SelectedChannel?.Id))
       {
            statusService.AddReport("getSelectedChannelUsers: "+ item.DisplayName);
            SelectedChannelUsers.Add(item);
       }
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