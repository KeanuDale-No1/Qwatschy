using VoiceChat.Data.Repositories;

namespace VoiceChat.Api.Endpoints
{
    public static class Endpoints
    {
        public static void AddEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Login", async (IRepository<Person> repo) =>
            {
                return await repo.GetAllAsync();
            });

            app.MapPost("/personen", async (IRepository<Person> repo, Person p) =>
            {
                await repo.AddAsync(p);
                await repo.SaveAsync();
                return Results.Created($"/personen/{p.Id}", p);
            });
        }
    }
}
