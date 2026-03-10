#region default
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast =  Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

//app.Run();

//record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
#endregion



using Microsoft.EntityFrameworkCore;
using VoiceChat.Api.Endpoints;
using VoiceChat.Api.Services;
using VoiceChat.Api.UseCases;
using VoiceChat.Data;
using VoiceChat.Data.Repositories;



var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddDbContext<VoiceChatDbContext>();
//builder.Services.AddRepositories();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddUsecases(); // sauber
builder.Services.AddSingleton<IAuthService, AuthService>();

// Register channels service (file-based persistence)
builder.Services.AddSingleton<ChannelsService>();
var app = builder.Build();



using var db = new VoiceChatDbContext();
db.Database.Migrate();

app.AddEndpoints();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseWebSockets();
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        await WebSocketHandler.Handle(socket);
    }
});




app.Run();
