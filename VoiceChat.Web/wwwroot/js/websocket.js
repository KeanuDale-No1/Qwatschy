let socket = null;
let dotNetRef = null;

export function createSocket(url, dotNetObject) {
    if (socket) {
        try { socket.close(); } catch {}
        socket = null;
    }
    dotNetRef = dotNetObject;
    socket = new WebSocket(url);

    socket.onopen = () => {
        if (dotNetRef) dotNetRef.invokeMethodAsync('OnOpen');
    };

    socket.onmessage = (e) => {
        if (dotNetRef) dotNetRef.invokeMethodAsync('ReceiveMessage', e.data);
    };

    socket.onclose = (e) => {
        if (dotNetRef) dotNetRef.invokeMethodAsync('OnClose', e.code, e.reason);
    };

    socket.onerror = (e) => {
        if (dotNetRef) dotNetRef.invokeMethodAsync('OnError');
    };
}

export function sendMessage(json) {
    if (!socket || socket.readyState !== WebSocket.OPEN) {
        throw 'Not connected';
    }
    socket.send(json);
}

export function closeSocket() {
    if (socket) {
        try { socket.close(); } catch {}
        socket = null;
    }
    if (dotNetRef) {
        try { dotNetRef.dispose(); } catch {}
        dotNetRef = null;
    }
}
