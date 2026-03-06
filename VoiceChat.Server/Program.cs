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


var builder = WebApplication.CreateBuilder(args);

// Register channels service (file-based persistence)
builder.Services.AddSingleton<ChannelsService>();

var app = builder.Build();

app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        await WebSocketHandler.Handle(socket);
    }
});

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

app.Run();