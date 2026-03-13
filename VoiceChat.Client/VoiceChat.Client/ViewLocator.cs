using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using VoiceChat.Client.ViewModels;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client;
    public class ViewLocator : IDataTemplate
    {

    private readonly IServiceProvider _services;

    public ViewLocator(IServiceProvider services)
    {
        _services = services;
    }
    public Control? Build(object? param)
        {
            if (param is null)
                return null;

            var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
