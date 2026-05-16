using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace VoiceChat.Client.Extensions;

public class ResolveExtension
{
    public Type? ViewModelType { get; set; }

    public ResolveExtension() { }

    public ResolveExtension(Type viewModelType)
    {
        ViewModelType = viewModelType;
    }

    public object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (ViewModelType == null)
            return null;

        return App.Services.GetRequiredService(ViewModelType);
    }
}