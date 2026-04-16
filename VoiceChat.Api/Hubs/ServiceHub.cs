using Microsoft.AspNetCore.SignalR;
using VoiceChat.Api.UseCases;
using VoiceChat.Shared.DTOs;
using VoiceChat.Shared.Networking;


namespace VoiceChat.Api.Hubs;

public class ChatHub() : Hub, IChatHubExchange
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        // Optional: Logik für neue Verbindungen
        Console.WriteLine("(Identity?.Name)Neue verbindung mit: " + Context?.User?.Identity?.Name);
        Console.WriteLine("(Context?.ConnectionId)Neue verbindung mit: " + Context?.ConnectionId);
        Console.WriteLine("(Context?.UserIdentifier)Neue verbindung mit: " + Context?.UserIdentifier);
        Console.WriteLine("(IP)Neue verbindung mit: " + Context?.User?.FindFirst("ip")?.Value);

    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        // Optional: Logik für Verbindungsabbrüche
        Console.WriteLine("(Identity?.Name)Neue geschlossen mit: " + Context?.User?.Identity?.Name);
        Console.WriteLine("(Context?.ConnectionId)Neue geschlossen mit: " + Context?.ConnectionId);
        Console.WriteLine("(Context?.UserIdentifier)Neue geschlossen mit: " + Context?.UserIdentifier);
        Console.WriteLine("(IP)Neue verbindung mit: " + Context?.User?.FindFirst("ip")?.Value);
    }

}
