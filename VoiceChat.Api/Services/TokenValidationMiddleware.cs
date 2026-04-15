using System.Security.Claims;
using VoiceChat.Api.Endpoints;

namespace VoiceChat.Api.Services;




public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthService handler)
    {
        // Skip auth for SignalR hub connections (they use query string token)
        if (context.Request.Path.StartsWithSegments("/connection"))
        {
            await _next(context);
            return;
        }

        // Nur prüfen, wenn der Endpoint Auth benötigt
        if (context.Request.Path.StartsWithSegments(""))
        {
            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.StartsWithSegments("/audio"))
            {
                var token = context.Request.Query.FirstOrDefault(x => x.Key == "token").Value.ToString();
                context.Request.Headers.TryAdd("Authorization", "Bearer "+token);
            }

            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Missing Authorization header");
                return;
            }

           

            try
            {
                var principal = handler.ValidateToken(authHeader!);
                context.User = principal;
                var ip = context.Connection.RemoteIpAddress?.ToString();

                // Optional: IP als Claim hinzufügen
                var identity = (ClaimsIdentity)principal.Identity!;
                identity.AddClaim(new Claim("ip", ip ?? "unknown"));
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token");
                return;
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Internal error");
                return;
            }
        }
       

        await _next(context);
    }
}
