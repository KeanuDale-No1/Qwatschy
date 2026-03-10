using VoiceChat.Data.Repositories;

namespace VoiceChat.Api.Endpoints
{

    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute { }



    public static class EndpointsRegistration
    {
        public static void AddEndpoints(this IEndpointRouteBuilder app)
        {
            app.AddAuthEndpoints();


            // Simple HTTP API for channel management (file-backed)
            app.MapGet("/api/channels", (ChannelsService svc) => svc.GetAllAsync());

            app.MapPost("/api/channels", async (ChannelsService svc, VoiceChat.Shared.Models.Channel channel) =>
            {
                var created = await svc.AddAsync(channel);
                return Results.Created($"/api/channels/{created.Id}", created);
            });

            app.MapDelete("/api/channels/{id:guid}", async (ChannelsService svc, Guid id) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}
