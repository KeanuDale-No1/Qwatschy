using Avalonia.Controls;
using System;
using VoiceChat.Client.ViewModels.MainArea;

namespace VoiceChat.Client.Views.MainArea;

public partial class MainAreaView : UserControl
{
    public MainAreaView()
    {
        InitializeComponent();
        Initialized += OnInitialized;

    }

    private async void OnInitialized(object? sender, EventArgs e)
    {
        if (DataContext is MainAreaViewModel vm)
        {
            await vm.Init();
        }
    }
}