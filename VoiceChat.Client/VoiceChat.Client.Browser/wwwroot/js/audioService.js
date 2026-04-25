let _ctx = null;
let _stream = null;
let _worklet = null;
let _dotnetExports = null;
let _jitterBuffer = [];
let _isPlaying = false;

export async function init() {
    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    _dotnetExports = await getAssemblyExports("Qwatschy.Client.Browser.dll");
    
    _ctx = new AudioContext({ sampleRate: 48000, latencyHint: "interactive" });
    
    console.log("[audioService] init complete (PCM mode)");
}

export async function startRecording() {
    if (_ctx && _ctx.state === 'suspended') {
        await _ctx.resume();
    }

    const stream = await navigator.mediaDevices.getUserMedia({
        audio: {
            echoCancellation: false,
            noiseSuppression: false,
            autoGainControl: false,
            channelCount: 1,
            sampleRate: 48000,
            googEchoCancellation: false,
            googAutoGainControl: false,
            googNoiseSuppression: false,
            googHighpassFilter: false
        }
    });

    _stream = stream;
    
    await _ctx.audioWorklet.addModule("js/audioProcessor.js");

    const source = _ctx.createMediaStreamSource(stream);
    const worklet = new AudioWorkletNode(_ctx, "pcm-processor");

    worklet.port.onmessage = async (e) => {
        const pcmBytes = e.data;
        _dotnetExports.VoiceChat.Client.Browser.Services.BrowserVoiceService.OnPcmFrame(pcmBytes);
    };

    source.connect(worklet);
    _worklet = worklet;
    
    console.log("[audioService] Recording started");
}

export function stopRecording() {
    if (_worklet) {
        _worklet.disconnect();
        _worklet = null;
    }
    if (_stream) {
        _stream.getTracks().forEach(t => t.stop());
        _stream = null;
    }
    console.log("[audioService] Recording stopped");
}

export function decodeAndPlayPCM(pcmBytes) {
    if (!_ctx) {
        console.log("[decodeAndPlayPCM] No AudioContext!");
        return;
    }

    if (_ctx.state === 'suspended') {
        _ctx.resume();
    }

    const pcm16 = new Int16Array(pcmBytes.buffer);
    const float32 = new Float32Array(pcm16.length);
    for (let i = 0; i < pcm16.length; i++) {
        float32[i] = pcm16[i] / 32767;
    }

    queueAudio(float32);
}

export function playOpusChunk(opusBytes) {
    decodeAndPlayPCM(opusBytes);
}

function queueAudio(float32) {
    _jitterBuffer.push(float32);
    playNext();
}

function playNext() {
    if (!_ctx || _isPlaying || _jitterBuffer.length === 0) {
        return;
    }

    const buf = _jitterBuffer.shift();
    if (!buf || buf.length === 0) {
        playNext();
        return;
    }
    
    _isPlaying = true;
    
    try {
        const audioBuffer = _ctx.createBuffer(1, buf.length, 48000);
        audioBuffer.copyToChannel(buf, 0);

        const source = _ctx.createBufferSource();
        source.buffer = audioBuffer;
        source.connect(_ctx.destination);

        source.onended = () => {
            _isPlaying = false;
            playNext();
        };

        source.start();
    } catch (e) {
        console.error("[playNext] Error:", e);
        _isPlaying = false;
        playNext();
    }
}

export function getLatency() {
    return _jitterBuffer.length;
}

export function clearBuffer() {
    _jitterBuffer = [];
    _isPlaying = false;
}