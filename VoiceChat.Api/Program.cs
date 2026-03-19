using Microsoft.EntityFrameworkCore;
using VoiceChat.Api.Endpoints;
using VoiceChat.Api.Hubs;
using VoiceChat.Api.Services;
using VoiceChat.Api.UseCases;
using VoiceChat.Data;
using VoiceChat.Data.Repositories;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VoiceChatDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddUsecases(); 
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
             .AllowAnyOrigin()
             .AllowAnyHeader()
             .AllowAnyMethod();
    });
});
builder.Services.AddSignalR();

var app = builder.Build();

using var db = new VoiceChatDbContext();
db.Database.Migrate();
app.UseCors();

app.AddEndpoints();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseWebSockets();

app.MapHub<ChatHub>("/connection");

//app.Map("/ws", async context =>
//{
//    if (context.WebSockets.IsWebSocketRequest)
//    {
//        var socket = await context.WebSockets.AcceptWebSocketAsync();
//        await WebSocketHandler.Handle(socket);
//    }
//});



app.Run();
