class PCMProcessor extends AudioWorkletProcessor {
    constructor() {
        super();
        this.buffer = [];
        this.chunkBuffer = [];
        this.FRAME_SIZE = 960;
        this.NOISE_THRESHOLD = 0.015;
        this.hpAlpha = 0.995;
        this.prevInput = 0;
        this.prevOutput = 0;
    }

    process(inputs, outputs, parameters) {
        const input = inputs[0][0];
        if (!input) return true;

        for (let i = 0; i < input.length; i++) {
            let sample = input[i];

            sample = this.highPassFilter(sample);

            this.buffer.push(sample);
        }

        while (this.buffer.length >= this.FRAME_SIZE) {
            const frame = this.buffer.slice(0, this.FRAME_SIZE);
            this.buffer = this.buffer.slice(this.FRAME_SIZE);

            const processed = this.applyNoiseGate(frame);

            if (this.isVoice(processed)) {
                this.encodeAndSend(processed);
            }
        }

        return true;
    }

    highPassFilter(sample) {
        const output = this.hpAlpha * (this.prevOutput + sample - this.prevInput);
        this.prevInput = sample;
        this.prevOutput = output;
        return output;
    }

    applyNoiseGate(frame) {
        let rms = 0;
        for (let i = 0; i < frame.length; i++) {
            rms += frame[i] * frame[i];
        }
        rms = Math.sqrt(rms / frame.length);

        if (rms < this.NOISE_THRESHOLD) {
            for (let i = 0; i < frame.length; i++) {
                frame[i] *= 0.3;
            }
        }
        return frame;
    }

    isVoice(frame) {
        let rms = 0;
        for (let i = 0; i < frame.length; i++) {
            rms += frame[i] * frame[i];
        }
        rms = Math.sqrt(rms / frame.length);
        return rms >= this.NOISE_THRESHOLD;
    }

    encodeAndSend(frame) {
        const pcm16 = new Int16Array(frame.length);
        for (let i = 0; i < frame.length; i++) {
            let s = Math.max(-1, Math.min(1, frame[i]));
            pcm16[i] = s * 32767;
        }

        const pcmBytes = new Uint8Array(pcm16.buffer);
        this.port.postMessage(pcmBytes);
    }
}

registerProcessor("pcm-processor", PCMProcessor);