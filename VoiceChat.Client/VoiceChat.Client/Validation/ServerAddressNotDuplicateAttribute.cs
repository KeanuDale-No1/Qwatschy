using System;
using System.ComponentModel.DataAnnotations;
using VoiceChat.Client.Services;

namespace VoiceChat.Client.Validation;

public sealed class ServerAddressNotDuplicateAttribute : ValidationAttribute
{
    public ServerAddressNotDuplicateAttribute()
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is string address && !string.IsNullOrWhiteSpace(address))
        {
            if (ServiceLocator.AppSettingsService?.AppSetting.Servers.ServerAddresses.Exists(s => s.ServerAddress == address) == true)
            {
                return false;
            }
        }
        return true;
    }
}
