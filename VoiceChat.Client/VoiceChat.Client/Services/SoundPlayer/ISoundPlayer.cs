using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Client.Services.SoundPlayer;

public interface ISoundPlayer
{
   public void Play(byte[] wav);
}
