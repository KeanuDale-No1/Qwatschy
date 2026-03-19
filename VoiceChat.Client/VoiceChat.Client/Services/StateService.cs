using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Client.Services;

public class StateService
{
    //public Guid UserId { get; private set; }
    //public string? UserName { get; private set; }

    public string? ServerAddress { get; private set; }
    public bool IsServerConnected { get; private set; }

    public Guid? SelectChannelId { get; private set; }
    public Guid? ConnectedChannelId { get; private set; }

    public bool IsMuted { get; private set; }
    public bool IsDeafened { get; private set; }

    public void SetConnectedServer(Guid UserId, string Username, string serverAddress)
    {
        ServerAddress = serverAddress;
        IsServerConnected = true;
    }

    public void SetDisconnectedServer()
    {
        ServerAddress = null;
        IsServerConnected = false;
    }

    public void SetSelectedChannel(Guid? channelId)
    {
        SelectChannelId = channelId;
    }

    public void SetConnectedChannel(Guid? channelId)
    {
        ConnectedChannelId = channelId;
    }

    public void SetMuted(bool isMuted)
    {
        IsMuted = isMuted;
    }

    public void SetDeafened(bool isDeafened)
    {
        IsDeafened = isDeafened;
    }


    public void ResetState()
    {
        ServerAddress = null;
        IsServerConnected = false;
        SelectChannelId = null;
        ConnectedChannelId = null;
        IsMuted = false;
        IsDeafened = false;
    }
}
