class PCMWorklet extends AudioWorkletProcessor {
    constructor() {
        super();
        this.buffer = [];
        this.chunkBuffer = [];        // Für JSInterop-Chunks
        this.FRAME_SIZE = 128;        // Samples pro Frame
        this.BATCH_SIZE = 8;          // Frames pro Chunk = 1024 Samples
    }

    process(inputs, outputs, parameters) {
        const input = inputs[0][0]; // Mono-Audio

        if (!input) return true;

        // 1️⃣ Samples in den Worklet-Puffer speichern
        this.buffer.push(...input);

        // 2️⃣ Solange genug für einen Frame vorhanden ist:
        while (this.buffer.length >= this.FRAME_SIZE) {
            const frame = this.buffer.slice(0, this.FRAME_SIZE);
            this.buffer = this.buffer.slice(this.FRAME_SIZE);

            // Float32 -> Int16 konvertieren
            const pcm16 = new Int16Array(this.FRAME_SIZE);
            for (let i = 0; i < this.FRAME_SIZE; i++) {
                let s = Math.max(-1, Math.min(1, frame[i]));
                pcm16[i] = s * 32767;
            }

            // 3️⃣ In Chunk-Buffer sammeln
            this.chunkBuffer.push(...pcm16);

            // 4️⃣ Wenn genug Samples gesammelt wurden, senden
            if (this.chunkBuffer.length >= this.FRAME_SIZE * this.BATCH_SIZE) {
                const bigChunk = new Int16Array(this.chunkBuffer);
                this.chunkBuffer = [];
                this.port.postMessage(bigChunk);  // JSInterop-freundlicher Call
            }
        }

        return true; // Worklet weiterlaufen lassen
    }
}

registerProcessor("pcm-worklet", PCMWorklet);
