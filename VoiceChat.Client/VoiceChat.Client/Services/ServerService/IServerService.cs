using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services.ServerService;

public interface IServerService
{
    public Task<bool> AddServer(string serverAdress);
    public Task CheckAlive();
}
