using Microsoft.AspNetCore.Mvc;
using VoiceChat.Api.Services;
using VoiceChat.Data.Repositories;
using VoiceChat.Domain.Auth;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.Endpoints;

public static class ServerEndpoints
{
    private static readonly string ServerImgPath = Path.Combine(AppContext.BaseDirectory, "ServerImg");

    public static void AddServerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/server/connect", async (
            IRepository<User> repository,
            IAuthService authService,
            ServerOptions serverOptions,
            LoginRequestDTO request) =>
        {
            if (Guid.Empty == request.ClientId)
                throw new ArgumentNullException(nameof(request.ClientId), "Es konnte keine ClientId ermittelt werden");

            var user = await repository.GetByIdAsync(request.ClientId);
            if (user == null)
            {
                user = new User
                {
                    Id = request.ClientId,
                    Username = request.DisplayName ?? $"User-{request.ClientId}"
                };
                await repository.AddAsync(user);
            }
            else if (!string.IsNullOrEmpty(request.DisplayName))
            {
                user.Username = request.DisplayName;
            }
            user.LastActive = DateTime.UtcNow;
            await repository.SaveAsync();

            var token = authService.GenerateToken(user.Id.ToString());
            var serverImage = GetServerImage();

            return new ServerConnectResponseDTO(
                token,
                serverOptions.ServerName,
                serverImage,
                serverOptions.ServerIcon
            );
        }).WithMetadata(new AllowAnonymousAttribute());
    }

    private static string? GetServerImage()
    {
        if (!Directory.Exists(ServerImgPath))
            return null;

        var imageFiles = Directory.GetFiles(ServerImgPath)
            .Where(f => new[] { ".png", ".jpg", ".jpeg", ".gif", ".webp" }
                .Contains(Path.GetExtension(f).ToLowerInvariant()))
            .ToList();

        if (imageFiles.Count == 0)
            return null;

        var imagePath = imageFiles.First();
        var imageBytes = File.ReadAllBytes(imagePath);
        var base64 = Convert.ToBase64String(imageBytes);
        var extension = Path.GetExtension(imagePath).TrimStart('.').ToLowerInvariant();
        var mimeType = extension switch
        {
            "png" => "image/png",
            "jpg" or "jpeg" => "image/jpeg",
            "gif" => "image/gif",
            "webp" => "image/webp",
            _ => "image/png"
        };

        return $"data:{mimeType};base64,{base64}";
    }
}
