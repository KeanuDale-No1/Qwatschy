using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using VoiceChat.Shared.Networking;

namespace VoiceChat.Web.Networking
{
    public class WebSocketService : IWebSocketService, IAsyncDisposable
    {
        readonly IJSRuntime _js;
        IJSObjectReference? _module;
        DotNetObjectReference<WebSocketService>? _dotNetRef;

        public event Action<SocketMessage>? OnMessage;

        public bool IsConnected { get; private set; }

        public WebSocketService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task ConnectAsync(Uri uri, CancellationToken ct = default)
        {
            if (IsConnected) return;

            _module ??= await _js.InvokeAsync<IJSObjectReference>("import", "/js/websocket.js");
            _dotNetRef = DotNetObjectReference.Create(this);
            await _module.InvokeVoidAsync("createSocket", uri.ToString(), _dotNetRef);
        }

        [JSInvokable]
        public Task ReceiveMessage(string json)
        {
            try
            {
                var msg = JsonSerializer.Deserialize<SocketMessage>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (msg != null) OnMessage?.Invoke(msg);
            }
            catch { }
            return Task.CompletedTask;
        }

        [JSInvokable]
        public Task OnOpen()
        {
            IsConnected = true;
            return Task.CompletedTask;
        }

        [JSInvokable]
        public Task OnClose(int code, string reason)
        {
            IsConnected = false;
            return Task.CompletedTask;
        }

        [JSInvokable]
        public Task OnError()
        {
            IsConnected = false;
            return Task.CompletedTask;
        }

        public async Task SendAsync(SocketMessage msg, CancellationToken ct = default)
        {
            if (!IsConnected) throw new InvalidOperationException("Not connected");
            var json = JsonSerializer.Serialize(msg);
            await _module!.InvokeVoidAsync("sendMessage", json);
        }

        public async Task DisconnectAsync()
        {
            try
            {
                if (_module != null) await _module.InvokeVoidAsync("closeSocket");
            }
            catch { }

            IsConnected = false;
            _dotNetRef?.Dispose();
            _dotNetRef = null;
        }

        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync();
            if (_module != null)
            {
                await _module.DisposeAsync();
                _module = null;
            }
        }
    }
}
