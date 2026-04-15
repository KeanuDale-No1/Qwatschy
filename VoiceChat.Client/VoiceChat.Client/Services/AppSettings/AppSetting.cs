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
    public Servers Servers { get; set; } = new Servers();
}

public class UserSettings
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
}

public class Servers
{
    public List<ServerSettings> ServerAddresses { get; set; } = new List<ServerSettings>();
}


public class ServerSettings
{
    public Guid ServerId { get; set; }
    public required string ServerAddress { get; set; }
}

[JsonSerializable(typeof(AppSetting))]
[JsonSerializable(typeof(UserSettings))]
[JsonSerializable(typeof(Servers))]
[JsonSerializable(typeof(ServerSettings))]
public partial class AppSettingContext : JsonSerializerContext
{
}