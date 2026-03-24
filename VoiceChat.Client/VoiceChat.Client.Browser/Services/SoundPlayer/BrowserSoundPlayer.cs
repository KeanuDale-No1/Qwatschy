using Microsoft.JSInterop;
using System;
using System.Runtime.Versioning;
using VoiceChat.Client.Services.SoundPlayer;

namespace VoiceChat.Client.Browser.Services.SoundPlayer;

public class BrowserSoundPlayer : ISoundPlayer
{
    private readonly IJSRuntime _js;

    public BrowserSoundPlayer(IJSRuntime js)
    {
        _js = js;
    }

    public void Play(byte[] wav)
    {
        _js.InvokeVoidAsync("playWav", Convert.ToBase64String(wav));
    }
}