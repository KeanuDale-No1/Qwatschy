using Microsoft.EntityFrameworkCore;
using VoiceChat.Api.Endpoints;
using VoiceChat.Api.Hubs;
using VoiceChat.Api.Services;
using VoiceChat.Api.UseCases;
using VoiceChat.Data;
using VoiceChat.Data.Repositories;

Console.WriteLine("Starting API...");

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Configuring services...");

builder.Services.AddDbContext<VoiceChatDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddUsecases(); 
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
             .SetIsOriginAllowed(_ => true)
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
    });
});
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

Console.WriteLine("Building app...");

var app = builder.Build();

Console.WriteLine("Running migrations...");
try
{
    using var db = new VoiceChatDbContext();
    Console.WriteLine("Database path: " + Path.Combine(Directory.GetCurrentDirectory(), "voicechat.db"));
    
    if (db.Database.GetPendingMigrations().Any())
    {
        Console.WriteLine("Executing migrate...");
        db.Database.Migrate();
        Console.WriteLine("Migrations complete.");
    }
    else
    {
        Console.WriteLine("No pending migrations, skipping.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Migration error: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}

Console.WriteLine("Continuing...");

app.UseCors();

app.AddEndpoints();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseWebSockets();

app.MapHub<ChatHub>("/connection");

Console.WriteLine("API ready. Listening on http://localhost:5000");

app.Run();
