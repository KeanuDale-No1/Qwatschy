using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services.AppSettings;

public class AppSetting
{
    public UserSettings UserSettings { get; set; } = new UserSettings();
    public ServerSettings ServerSettings { get; set; } = new ServerSettings();
}

public class UserSettings
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
}

public class ServerSettings
{
    public List<string> ServerAddress { get; set; } = new List<string>();
}


[JsonSerializable(typeof(AppSetting))]
[JsonSerializable(typeof(UserSettings))]
[JsonSerializable(typeof(ServerSettings))]
public partial class AppSettingContext : JsonSerializerContext
{
}