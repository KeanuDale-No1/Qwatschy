using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Client.Models;

public class ChannelInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Desciption { get; set; }

    public ObservableCollection<ConnectedUser> ConnectedUsers = new();
}