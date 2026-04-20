using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VoiceChat.Client.Hubs;

namespace VoiceChat.Client.Models;

public partial class ServerConnectionInfo : ObservableObject
{
    public ServerConnectionInfo(Guid serverId, string serverAdress, string serverName)
    {
        ServerId = serverId;
        ServerAdress = serverAdress;
        ServerName = serverName;
    }

    public Guid ServerId { get; set; }
    public string ServerAdress { get; set; }
    public string ServerName { get; set; }
    [ObservableProperty] private string? serverImage;


    public string? Description { get; set; }
    public string? Token { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    [ObservableProperty] private bool isConnected;

    public string Abbr => AbbreviationHelper.GetAbbreviation(ServerName);


    public ObservableCollection<ChannelInfo> ChannelInfos = new();

}

public static class AbbreviationHelper
{
    public static string GetAbbreviation(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return new string(input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length > 2)
            .SelectMany(w => w.Where(char.IsUpper))
            .Take(3)
            .ToArray());
    }
}