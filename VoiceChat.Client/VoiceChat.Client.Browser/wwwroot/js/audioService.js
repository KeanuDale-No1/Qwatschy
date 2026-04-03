export async function startRecording() {
    const stream = await navigator.mediaDevices.getUserMedia({
        audio: {
            echoCancellation: true,
            noiseSuppression: true,
            autoGainControl: true,
            channelCount: 1,
            sampleRate: 48000
        }
    });

    const ctx = new AudioContext({ sampleRate: 48000 });
    const source = ctx.createMediaStreamSource(stream);
    const processor = ctx.createScriptProcessor(2048, 1, 1);

    processor.onaudioprocess = async (e) => {
        const input = e.inputBuffer.getChannelData(0);

        const pcm16 = new Int16Array(960);
        for (let i = 0; i < 960; i++) {
            let s = input[i];
            if (s > 1) s = 1;
            if (s < -1) s = -1;
            pcm16[i] = s * 32767;
        }

        const bytes = new Uint8Array(pcm16.buffer);

        const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
        const exports = await getAssemblyExports("VoiceChat.Client.Browser.dll");
        exports.VoiceChat.Client.Browser.Services.BrowserVoiceService.OnPcmFrame(bytes);
    };

    source.connect(processor);
    processor.connect(ctx.destination);

    globalThis._ctx = ctx;
    globalThis._proc = processor;
    globalThis._stream = stream;
}

export function stopRecording() {
    if (globalThis._proc) {
        globalThis._proc.disconnect();
        globalThis._proc = null;
    }
    if (globalThis._ctx) {
        globalThis._ctx.close();
        globalThis._ctx = null;
    }
    if (globalThis._stream) {
        globalThis._stream.getTracks().forEach(t => t.stop());
        globalThis._stream = null;
    }
}
