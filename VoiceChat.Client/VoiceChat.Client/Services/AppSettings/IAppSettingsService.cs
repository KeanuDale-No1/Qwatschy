using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services.AppSettings;

public interface IAppSettingsService
{
    public AppSetting AppSetting { get; set; }

    public void InitAppSettings();
    public void SaveAppSettings();
}
