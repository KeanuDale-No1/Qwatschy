using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using System;
using VoiceChat.Client.ViewModels;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;


        // Fall 1: param ist ein Type → per DI auflösen
        if (param is Type vmType && typeof(ViewModelBase).IsAssignableFrom(vmType))
        {
            var vm = App.Services.GetRequiredService(vmType);
            return BuildFromInstance((ViewModelBase)vm);
        }
        // Fall 2: param ist eine ViewModel-Instanz (aktueller Flow)
        if (param is ViewModelBase vmInstance)
            return BuildFromInstance(vmInstance);


      // var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
      // var type = Type.GetType(name);
      //
      // if (type != null)
      // {
      //     return (Control)Activator.CreateInstance(type)!;
      // }

        return new TextBlock { Text = "Not Found: " + param };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }


    private Control BuildFromInstance(ViewModelBase vm)
    {
        var name = vm.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
            return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = "Not Found: " + name };
    }

}
