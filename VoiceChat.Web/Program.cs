using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VoiceChat.Web;
using VoiceChat.Web.Networking;
using VoiceChat.Shared.Networking;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// Register the WebSocket service for the Blazor client
builder.Services.AddScoped<IWebSocketService, WebSocketService>();
// Bind WebSocket configuration from wwwroot/appsettings.json (use shared options type)
builder.Services.Configure<VoiceChat.Shared.Networking.WebSocketOptions>(builder.Configuration.GetSection("WebSocket"));

await builder.Build().RunAsync();
