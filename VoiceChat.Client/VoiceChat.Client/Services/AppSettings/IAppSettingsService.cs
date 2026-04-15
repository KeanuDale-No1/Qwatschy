using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services.AppSettings;

public interface IAppSettingsService
{
    public AppSetting AppSetting { get; }
    public bool NewAppSetting { get; }
    public void InitAppSettings();
    public void SetUsername(string Username);
    public void AddServer(Guid serverId, string serverAdress);
    public void RemoveServer(Guid serverId);

}