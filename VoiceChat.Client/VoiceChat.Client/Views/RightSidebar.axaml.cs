using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using VoiceChat.Client.ViewModels;

namespace VoiceChat.Client.Views;

public partial class RightSidebar : UserControl
{
    public RightSidebar()
    {
        InitializeComponent();
        if (!Design.IsDesignMode)
        {
            DataContext = App.Services.GetRequiredService<RightSidebarViewModel>();
        }
    }
}