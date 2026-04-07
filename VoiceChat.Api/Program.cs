// Copyright (c) 2026 KeanuDale-No1 - All Rights Reserved
// Unauthorized copying, modification, or distribution is strictly prohibited

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VoiceChat.Api.Endpoints;
using VoiceChat.Api.Hubs;
using VoiceChat.Api.Services;
using VoiceChat.Api.UseCases;
using VoiceChat.Api.WebSockets;
using VoiceChat.Data;
using VoiceChat.Data.Repositories;
using VoiceChat.Shared.Models;

Console.WriteLine("Starting API...");

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Configuring services...");

builder.Services.AddDbContext<VoiceChatDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddUsecases();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtOptions>>().Value);
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

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>()!;

// Audio WebSocket endpoint
var audioHandler = new AudioWebSocketHandler();

app.Map("/audio", async context =>
{
    Console.WriteLine($"[/audio] Request received from: {context.Connection.RemoteIpAddress}");
    Console.WriteLine($"[/audio] IsWebSocketRequest: {context.WebSockets.IsWebSocketRequest}");
    Console.WriteLine($"[/audio] Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");

    if (!context.WebSockets.IsWebSocketRequest)
    {
        Console.WriteLine($"[/audio] Not a WebSocket request, returning 400");
        context.Response.StatusCode = 400;
    }

    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
    var channelId = context.Request.Query["channelId"].ToString();
    await audioHandler.HandleWebSocketAsync(channelId, webSocket, context.RequestAborted);
});//.AllowAnonymous();

Console.WriteLine("API ready. Listening on http://localhost:5000");

app.Run();
