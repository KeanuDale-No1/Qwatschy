using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Shared.DTOs;

namespace VoiceChat.Shared.Networking
{
    public interface IChatHubExchange
    {
        //Server Management
        Task OnConnectedAsync();

      
    }
}
