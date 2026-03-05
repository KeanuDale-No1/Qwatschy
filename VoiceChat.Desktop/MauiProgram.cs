using Microsoft.Extensions.Logging;
using VoiceChat.Desktop.Networking;
using VoiceChat.Shared.Networking;

namespace VoiceChat.Desktop;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Register WebSocket service for the app
        builder.Services.AddSingleton<IWebSocketService, WebSocketService>();

        return builder.Build();
	}
}
