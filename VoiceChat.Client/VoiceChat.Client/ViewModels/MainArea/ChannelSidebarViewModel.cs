using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.MainArea;

public class UserModel { public Guid ClientID { get; set; } public string DisplayName { get; set; } }
public class Channel { public Guid Id { get; set; } public string Name { get; set; } = ""; public string Description { get; set; } = ""; }

public partial class ChannelSidebarViewModel : ViewModelBase
{
    private readonly IHttpClientService httpClientService;
    private readonly StatusService statusService;

    public ObservableCollection<Channel> Channels { get; } = new ObservableCollection<Channel> { new Channel { Id = Guid.NewGuid(), Name = "Avalonia" }, new Channel { Id = Guid.NewGuid(), Name = "Gaming" } };

    public ChannelSidebarViewModel(IHttpClientService httpClientService, StatusService statusService)
    {
        this.statusService = statusService;
        this.httpClientService = httpClientService;
        LoadChannels();
    }

    public async void LoadChannels()
    {
        statusService.AddReport("Lade Kanäle...");
    }


    [RelayCommand]
    private async Task JoinChannel()
    {

    }


    [RelayCommand]
    public async Task CreateChannel()
    {
    }

    

    private async Task DeleteChannel()
    {
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



    public ObservableCollection<UserModel> OnlineUsers { get; set; } = new()
{
    new UserModel { ClientID = Guid.NewGuid(), DisplayName = "Alice" },
    new UserModel { ClientID= Guid.NewGuid(), DisplayName = "Alice2" },
    new UserModel { ClientID= Guid.NewGuid(), DisplayName = "Alice3" },
};
}