using Android.Media;
using System;
using System.Collections.Generic;
using VoiceChat.Client.Services.SoundPlayer;

namespace VoiceChat.Client.Android.Services.SoundPlayer;

public class AndroidSoundPlayer : ISoundPlayer
{
    public void Play(byte[] wav)
    {
        var track = new AudioTrack(
            Stream.Music,
            44100,
            ChannelOut.Mono,
            Encoding.Pcm16bit,
            wav.Length,
            AudioTrackMode.Static
        );

        track.Write(wav, 0, wav.Length);
        track.Play();
    }
}
