//using NAudio.Wave;
using System.IO;
using VoiceChat.Client.Services.SoundPlayer;

namespace VoiceChat.Client.Desktop.Services.SoundPlayer;

public class DesktopSoundPlayer : ISoundPlayer
{
    //private WaveOutEvent? _output;
    public void Play(byte[] wav)
    {

        //var ms = new MemoryStream(wav);
        //var reader = new WaveFileReader(ms);

        //_output?.Dispose();
        //_output = new WaveOutEvent();
        //_output.DeviceNumber = -1;
        //_output.Init(reader);
        //_output.Play();

        // using var ms = new MemoryStream(wav);
        // using var reader = new WaveFileReader(ms);
        // using var output = new WaveOutEvent();
        // output.Init(reader);
        // output.Play();
    }
}
