using Microsoft.EntityFrameworkCore;
using VoiceChat.Api.Services;
using VoiceChat.Data.Repositories;
using VoiceChat.Domain.Auth;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.UseCases;

public class LoginUsecase(IRepository<User> repository, IAuthService authService) : IUseCase<LoginRequestDTO, LoginResponseDTO>
{
    public async Task<LoginResponseDTO> ExecuteAsync(LoginRequestDTO request)
    {
        if (Guid.Empty == request.ClientId)
            throw new ArgumentNullException(nameof(request.ClientId), "Es konnte keine ClientId ermittelt werden");

        var user = await repository.GetByIdAsync(request.ClientId);
        if (user == null)
        {
            user = new User
            {
                Id = request.ClientId,
                DisplayName = request.Displayname ?? $"User-{request.ClientId.ToString()}"
            };
            await repository.AddAsync(user);
        }
        else if (!string.IsNullOrEmpty(request.Displayname))
        {
            user.DisplayName = request.Displayname;
        }
        user.LastActive = DateTime.UtcNow;

        await repository.SaveAsync();

        var token = authService.GenerateToken(user.Id.ToString());

        return new LoginResponseDTO(user.Id, user.DisplayName, token);
    }
}
