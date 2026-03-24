using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoiceChat.Client.Services.SoundPlayer;

namespace VoiceChat.Client.Utilitis;

public class Sounds()//ISoundPlayer soundPlayer)
{
    public void PlayJoinSound()
    {
        //var pcm = SoundGenerator.GenerateSineWave(frequency: 660, 
        //                                          durationSeconds: 0.12, 
        //                                          volume: 0.4);

        var pcm = SoundGenerator.GenerateSmoothUiSound();

        var wav = WavHelper.CreateWav(pcm);

       // soundPlayer.Play(wav);
    }

}

internal static class SoundGenerator
{
    internal static byte[] GenerateSineWave(
    int sampleRate = 44100,
    double frequency = 440.0,
    double durationSeconds = 0.15,
    double volume = 0.3)
    {
        int sampleCount = (int)(sampleRate * durationSeconds);
        byte[] buffer = new byte[sampleCount * 2]; // 16-bit PCM

        for (int i = 0; i < sampleCount; i++)
        {
            double t = (double)i / sampleRate;
            short sample = (short)(Math.Sin(2 * Math.PI * frequency * t) * short.MaxValue * volume);

            buffer[i * 2] = (byte)(sample & 0xFF);
            buffer[i * 2 + 1] = (byte)((sample >> 8) & 0xFF);
        }
        return buffer;
    }
    public static byte[] GenerateSmoothUiSound(double duration = 0.25, int sampleRate = 44100)
    {
        int sampleCount = (int)(duration * sampleRate);
        short[] samples = new short[sampleCount];

        double startFreq = 420.0;   // warm
        double endFreq = 620.0;     // smooth upward glide

        for (int i = 0; i < sampleCount; i++)
        {
            double t = (double)i / sampleRate;

            // Pitch glide
            double freq = startFreq + (endFreq - startFreq) * (i / (double)sampleCount);

            // Smooth sine
            double sample = Math.Sin(2 * Math.PI * freq * t);

            // Fade out
            double fade = 1.0 - (i / (double)sampleCount);

            // Soften attack
            double attack = Math.Min(1.0, i / (sampleRate * 0.01));

            samples[i] = (short)(sample * fade * attack * 3000);
        }

        // Convert to PCM16
        byte[] pcm = new byte[sampleCount * 2];
        Buffer.BlockCopy(samples, 0, pcm, 0, pcm.Length);



        return pcm;
    }

}

internal static class WavHelper
{
    internal static byte[] CreateWav(byte[] pcm, int sampleRate = 44100)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        int subchunk2Size = pcm.Length;
        int chunkSize = 36 + subchunk2Size;

        bw.Write("RIFF"u8);
        bw.Write(chunkSize);
        bw.Write("WAVE"u8);

        bw.Write("fmt "u8);
        bw.Write(16);
        bw.Write((short)1);
        bw.Write((short)1);
        bw.Write(sampleRate);
        bw.Write(sampleRate * 2);
        bw.Write((short)2);
        bw.Write((short)16);

        bw.Write("data"u8);
        bw.Write(subchunk2Size);
        bw.Write(pcm);

        return ms.ToArray();
    }
}
