using Concentus;
using Concentus.Enums;
using Concentus.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Client.Services.VoiceService
{
    public class OpusCodec
    {

        private readonly IOpusEncoder encoder = OpusCodecFactory.CreateEncoder(48000, 1, OpusApplication.OPUS_APPLICATION_VOIP);
        private readonly IOpusDecoder decoder = OpusCodecFactory.CreateDecoder(48000, 1);

        private readonly short[] pcmBuffer = new short[960]; //20ms bei 48kHz

        public byte[] Encode(short[] pcmData)
        {
            byte[] opusPacked = new byte[1275]; 
            
            int length = encoder.Encode(pcmData, 960, opusPacked, opusPacked.Length);
            Array.Resize(ref opusPacked, length); //Größe auf tatsächliche Länge anpassen
            return opusPacked[..length];
        }

        public short[] Decode(byte[] opusData)
        {
            int samples = decoder.Decode(opusData, pcmBuffer, pcmBuffer.Length);
            return pcmBuffer; //Nur die tatsächlich dekodierten Samples zurückgeben
        }

    }
}
