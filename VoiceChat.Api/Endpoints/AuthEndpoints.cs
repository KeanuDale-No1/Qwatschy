using Microsoft.AspNetCore.Mvc;
using VoiceChat.Api.Services;
using VoiceChat.Api.UseCases;
using VoiceChat.Data.Repositories;
using VoiceChat.Shared.Models;

namespace VoiceChat.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void AddEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Login", async (IUseCase<LoginRequest,LoginResponse> useCase, LoginRequest loginRequest) =>
            {
                return await useCase.ExecuteAsync(loginRequest);
            }).WithMetadata(new AllowAnonymousAttribute()); ;


            app.MapPost("api/validate", async (IAuthService authService, [FromHeader(Name = "Authorization")] string authorization) =>
            {

                if (string.IsNullOrWhiteSpace(authorization))
                    return Results.BadRequest("Missing Authorization header");

                try
                {
                    authService.ValidateToken(authorization);
                    return Results.Ok("Token is valid");
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Unauthorized();
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });
        }
    }
}
