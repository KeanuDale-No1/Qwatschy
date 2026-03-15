using AVFoundation;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Client.Services.SoundPlayer;

namespace VoiceChat.Client.iOS.Services.SoundPlayer;

public class IosSoundPlayer : ISoundPlayer
{
    public void Play(byte[] wav)
    {
        var data = NSData.FromArray(wav);
        var player = AVAudioPlayer.FromData(data);
        player.Play();
    }
}