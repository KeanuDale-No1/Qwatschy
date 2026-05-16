using System;
using System.Collections.ObjectModel;

namespace VoiceChat.Client.Models;

public class ChannelInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Desciption { get; set; }

    public int UnreadMessagesCount { get; set; } = 0;

    public ObservableCollection<ConnectedUser> ConnectedUsers { get; set; } = new();
}