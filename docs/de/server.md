---
title: Server
lang: de
layout: default
previous: installation
previous_text: Installation
next: getting-started
next_text: Erste Schritte
---

# Server

## Server aufsetzen

### Voraussetzungen
- .NET 10.0 SDK installiert
- Ports 5000 und 5001 verfügbar

### Server starten

**Option 1: Aus Quellcode**

```bash
# Repository klonen
git clone https://github.com/KeanuDale-No1/Qwatschy.git
cd Qwatschy

# Server starten
dotnet run --project VoiceChat.Api/VoiceChat.Api.csproj
```

**Option 2: Fertiger Server (Download)**

Lade den Server für dein Betriebssystem herunter:

| Plattform | Download |
|-----------|----------|
| Windows | [Qwatschy-Server-win.zip](https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-Server-win.zip) |
| Linux | [Qwatschy-Server-linux.zip](https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-Server-linux.zip) |

```bash
# Entpacken und starten (Linux)
unzip Qwatschy-Server-linux.zip
./Qwatschy/Qwatschy
```

Der Server startet standardmäßig auf `http://localhost:5000`.

## Server-Konfiguration

Die API-Konfiguration findest du in `VoiceChat.Api/appsettings.json`:

```json
{
  "Urls": "http://0.0.0.0:5000",
  "Jwt": {
    "Key": "DEIN_GEHEIMER_SCHLÜSSEL",
    "Issuer": "VoiceChat",
    "Audience": "VoiceChat"
  }
}
```

### Wichtige Einstellungen

| Einstellung | Standard | Beschreibung |
|-------------|----------|-------------|
| `Urls` | `http://0.0.0.0:5000` | Server-URL |
| `Jwt:Key` | (leer) | Geheimer Schlüssel für JWT |
| `Jwt:ExpirationMinutes` | 60 | Token-Lebensdauer |

### Sicherheit

- **JWT Key**: Ändere den Standard-Schlüssel in einem Produktivsystem!
- **Firewall**: Port 5000 für externe Verbindungen öffnen
- **HTTPS**: Für Produktion wird ein Reverse Proxy mit HTTPS empfohlen

## Server-Befehle

| Befehl | Beschreibung |
|--------|--------------|
| `dotnet run` | Server starten |
| `dotnet run --urls "http://*:5000"` | Auf allen Interfaces lauschen |
| `Ctrl+C` | Server stoppen |
