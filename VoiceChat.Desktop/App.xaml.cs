using Microsoft.Extensions.DependencyInjection;

namespace VoiceChat.Desktop;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Apply saved theme preference at startup (if any)
        try
        {
            var pref = Microsoft.Maui.Storage.Preferences.Get("display_mode", "system");
            switch ((pref ?? "system").ToLowerInvariant())
            {
                case "light":
                    Current.UserAppTheme = AppTheme.Light;
                    break;
                case "dark":
                    Current.UserAppTheme = AppTheme.Dark;
                    break;
                default:
                    Current.UserAppTheme = AppTheme.Unspecified;
                    break;
            }
        }
        catch { }
    }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}