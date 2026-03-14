using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Shared.Models;

namespace VoiceChat.Client.ViewModels.MainArea;

public class UserModel { public Guid ClientID { get; set; } public string DisplayName { get; set; } }
public class Channel { public Guid Id { get; set; } public string Name { get; set; } = ""; public string? Description { get; set; } = ""; }

public partial class ChannelSidebarViewModel : ViewModelBase
{
    private readonly IHttpClientService httpClientService;
    private readonly StatusService statusService;

    public ObservableCollection<Channel> Channels { get; set; } = new ObservableCollection<Channel>();

    public ChannelSidebarViewModel(IHttpClientService httpClientService, StatusService statusService)
    {
        this.statusService = statusService;
        this.httpClientService = httpClientService;
        LoadChannels();
    }

    public async void LoadChannels()
    {
        try
        {
            statusService.AddReport("Lade Kanäle...");
            var response = await httpClientService.PostAsync<GetChannelsRequestDTO, GetChannelsResponseDTO>("api/GetChannels", new GetChannelsRequestDTO());

            foreach (var item in response.Channels.Select(x => new Channel { Id = x.Id, Name = x.Name, Description = x.Description }))
            {
                Channels.Add(item);
            }
            
        }
        catch (Exception ex)
        {
            statusService.AddReport($"Fehler beim laden der Channels {ex.Message}");
        }
    }


    [RelayCommand]
    private async Task JoinChannel()
    {

    }


    [RelayCommand]
    public async Task CreateChannel()
    {
        try
        {
            ChannelDTO channel = new ChannelDTO() { Id = Guid.NewGuid(), Name ="Room 1" , Description =""};

            var response = await httpClientService.PostAsync<CreateChannelRequestDTO, CreateChannelRequestDTO>("api/CreateChannel", new CreateChannelRequestDTO(channel));
            if (response == null)
                statusService.AddReport("Channel konnte nicht erstellt werden:");
            else
                statusService.AddReport("Channel wurde erfolgreich erstellt");
        }
        catch (Exception ex)
        {
            statusService.AddReport($"Channel konnte nicht erstellt werden:{ex.Message}");
        }
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