using Microsoft.Maui.Storage;

namespace VoiceChat.Desktop;

public partial class SettingsPage : ContentPage
{
    const string PrefKey = "display_mode"; // values: system, light, dark

    public SettingsPage()
    {
        InitializeComponent();
        LoadPreference();
    }

    void LoadPreference()
    {
        var val = Preferences.Get(PrefKey, "system");
        switch (val?.ToLowerInvariant())
        {
            case "light":
                RbLight.IsChecked = true;
                Application.Current.UserAppTheme = AppTheme.Light;
                break;
            case "dark":
                RbDark.IsChecked = true;
                Application.Current.UserAppTheme = AppTheme.Dark;
                break;
            default:
                RbSystem.IsChecked = true;
                Application.Current.UserAppTheme = AppTheme.Unspecified;
                break;
        }
    }

    void OnRadioChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (!(sender is RadioButton rb) || !rb.IsChecked) return;

        var content = (rb.Content as string)?.ToLowerInvariant();
        switch (content)
        {
            case "whitemode":
            case "light":
                Preferences.Set(PrefKey, "light");
                Application.Current.UserAppTheme = AppTheme.Light;
                break;
            case "darkmode":
            case "dark":
                Preferences.Set(PrefKey, "dark");
                Application.Current.UserAppTheme = AppTheme.Dark;
                break;
            default:
                Preferences.Set(PrefKey, "system");
                Application.Current.UserAppTheme = AppTheme.Unspecified;
                break;
        }
    }
}
