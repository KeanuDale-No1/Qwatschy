using System;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using VoiceChat.Shared.Networking;

namespace VoiceChat.Desktop;

public partial class MainPage : ContentPage
{
    int count = 0;
    IWebSocketService? _ws;

    public MainPage()
    {
        InitializeComponent();

        // Resolve the IWebSocketService from the MAUI service provider
        try
        {
            _ws = Application.Current?.Handler?.MauiContext?.Services.GetService(typeof(IWebSocketService)) as IWebSocketService;
        }
        catch
        {
            // ignore resolution errors
        }
    }

    private async void OnCounterClicked(object? sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private async void OnConnectClicked(object? sender, EventArgs e)
    {
        // Read URL from the entry and attempt to connect and send a test message
        try
        {
            if (_ws == null)
            {
                Console.WriteLine("WebSocket service not available");
                return;
            }

            var text = ServerEntry?.Text;

            // If no entry provided, try to read from appsettings.json in the app folder
            if (string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    var file = Path.Combine(System.AppContext.BaseDirectory, "appsettings.json");
                    if (File.Exists(file))
                    {
                        var json = File.ReadAllText(file);
                        using var doc = JsonDocument.Parse(json);
                        if (doc.RootElement.TryGetProperty("WebSocket", out var wsEl) && wsEl.TryGetProperty("Url", out var urlEl))
                        {
                            text = urlEl.GetString();
                        }
                    }
                }
                catch { }
            }

            Uri uri;
            if (!string.IsNullOrWhiteSpace(text))
            {
                uri = new Uri(text);
            }
            else
            {
                // fallback
                uri = new Uri("ws://localhost:5085/ws");
            }

            if (!_ws.IsConnected)
            {
                await _ws.ConnectAsync(uri);
                await Task.Delay(100);
            }

            var msg = new VoiceChat.Shared.Networking.SocketMessage { Type = "client_message", Data = "HalloServver" };
            await _ws.SendAsync(msg);
            Console.WriteLine($"Connected to {uri} and sent test message");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connect error: {ex.Message}");
        }
    }
}
