using Microsoft.AspNetCore.SignalR;
using VoiceChat.Shared.Models;


namespace VoiceChat.Api.Hubs;

public class ChatHub:Hub
{
    public async Task SendMessage(ChatMessageDTO message)
    {
        // Optional: Validierung
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
