// Simple cached DOTNET exports to reduce bridge overhead per frame
let _dotnetExportsCache = null;
export async function init() {
    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    _dotnetExportsCache = await getAssemblyExports("VoiceChat.Client.Browser.dll");
}


export async function startRecording() {
    const stream = await navigator.mediaDevices.getUserMedia({
        audio: {
            echoCancellation: false,
            noiseSuppression: false,
            autoGainControl: false,
            channelCount: 1,
            sampleRate: 48000,

            // Chrome-spezifische Flags
            googEchoCancellation: false,
            googAutoGainControl: false,
            googNoiseSuppression: false,
            googHighpassFilter: false
        }
    });

    // Prefer 'interactive' latency for real-time capture
    const ctx = new AudioContext({ sampleRate: 48000, latencyHint: 0.01 });
    await ctx.audioWorklet.addModule("js/processor.js");

    const source = ctx.createMediaStreamSource(stream);
    const worklet = new AudioWorkletNode(ctx, "pcm-worklet");

    worklet.port.onmessage = async (e) => {
        const pcm16 = e.data;
        const bytes = new Uint8Array(pcm16.buffer);
        _dotnetExportsCache.VoiceChat.Client.Browser.Services.BrowserVoiceService.OnPcmFrame(bytes);
    };

    source.connect(worklet);

    globalThis._ctx = ctx;
    globalThis._stream = stream;
    globalThis._worklet = worklet;
}

export function stopRecording() {
    if (globalThis._worklet) {
        globalThis._worklet.disconnect();
        globalThis._worklet = null;
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


export function decodeAndPlayPCM(pcmBytes) {
    const pcm16 = new Int16Array(pcmBytes.buffer);

    const float32 = new Float32Array(pcm16.length);
    for (let i = 0; i < pcm16.length; i++) {
        float32[i] = pcm16[i] / 32767;
    }

    const audioBuffer = ctx.createBuffer(1, float32.length, 48000);
    audioBuffer.copyToChannel(float32, 0);

    const source = ctx.createBufferSource();
    source.buffer = audioBuffer;
    source.connect(ctx.destination);
    source.start();
}