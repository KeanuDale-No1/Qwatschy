using Microsoft.AspNetCore.SignalR;
using VoiceChat.Api.UseCases;
using VoiceChat.Api.UseCases.Channels.ChannelMessage;
using VoiceChat.Shared.Models;


namespace VoiceChat.Api.Hubs;

public class ChatHub(IUseCase<CreateChannelRequestDTO, CreateChannelResponseDTO> channelCreateUseCase,
                     IUseCase<DeleteChannelRequestDTO, DeleteChannelResponseDTO> deleteCreateUseCase,
                     IUseCase<ConnectChannelRequestDTO, ConnectChannelResponseDTO> joinChannelUseCase,
                     IUseCase<CreateChatMessageRequestDTO, CreateChatMessageResponseDTO> createChatMessageUseCase,
                     IUseCase<GetMessagesRequestDTO, GetMessagesResponseDTO> getMessagesUseCase) : Hub
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


    public async Task JoinChannel(Guid channelId)
    {
        // Optional: Validierung
        if (channelId == Guid.Empty)
            throw new ArgumentNullException(nameof(channelId));
        if (string.IsNullOrEmpty(Context.UserIdentifier))
            throw new UnauthorizedAccessException(nameof(Context.UserIdentifier));

        var response = await joinChannelUseCase.ExecuteAsync(new ConnectChannelRequestDTO(Guid.Parse(Context.UserIdentifier!), channelId));
        await Clients.All.SendAsync("JoinChannel", response);
        await Groups.AddToGroupAsync(Context.ConnectionId, channelId.ToString());
        
    }

    

    
    public async Task DeleteChannel(Guid channelId)
    {
        // Optional: Validierung
        if (channelId == Guid.Empty)
            throw new ArgumentNullException(nameof(channelId));

        var response = await deleteCreateUseCase.ExecuteAsync(new DeleteChannelRequestDTO(channelId));

        await Clients.All.SendAsync("DeleteChannelChange", channelId);
    }



    #region Channel and Message Management

    public async Task AddChannel(ChannelDTO message)
    {
        // Optional: Validierung
        if (message == null)
            throw new ArgumentNullException(nameof(message));
        var respnse = await channelCreateUseCase.ExecuteAsync(new CreateChannelRequestDTO(message));
        await Clients.All.SendAsync("AddChannelChange", respnse.Channel);
    }

    public async Task SendAudioFrame(Guid ChannelId, byte[] audioData)
    {
        await Clients.OthersInGroup(ChannelId.ToString()).SendAsync("ReceiveAudioFrame", Context.ConnectionId, audioData);
    }
    public async Task SendMessage(ChatMessageDTO message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));
        await createChatMessageUseCase.ExecuteAsync(new CreateChatMessageRequestDTO(new ChatMessageDTO(message.SenderId, message.ChannelId, message.Content, DateTime.UtcNow, Context?.User?.Identity?.Name)));

        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public async Task<GetMessagesResponseDTO> GetMessages(Guid channelId, int skip = 0, int take = 50)
    {
        return await getMessagesUseCase.ExecuteAsync(new GetMessagesRequestDTO(channelId, skip, take));
    }
    #endregion
}
