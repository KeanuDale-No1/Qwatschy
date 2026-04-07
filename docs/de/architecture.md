---
title: Architektur
lang: de
---

# Architektur

## Überblick

Qwatschy ist eine moderne, plattformübergreifende Anwendung mit einer Client-Server-Architektur.

```
┌─────────────────────────────────────────────────────────────┐
│                        CLIENTS                               │
├─────────────┬─────────────┬─────────────┬──────────────────┤
│   Desktop   │   Mobile    │   Browser   │      Other       │
│  (Avalonia) │  (MAUI)     │  (WASM)     │                  │
└──────┬──────┴──────┬──────┴──────┬──────┴────────┬─────────┘
       │             │             │               │
       └─────────────┴─────────────┴───────────────┘
                              │
                    ┌─────────▼─────────┐
                    │   VoiceChat.Api   │
                    │   (ASP.NET Core)  │
                    └─────────┬─────────┘
                              │
              ┌───────────────┼───────────────┐
              │               │               │
       ┌──────▼──────┐ ┌──────▼──────┐ ┌──────▼──────┐
       │  REST API   │ │  SignalR    │ │  WebSocket  │
       │  /api/*     │ │  /connection│ │  /audio     │
       └──────┬──────┘ └──────┬──────┘ └──────┬──────┘
              │               │               │
              └───────────────┼───────────────┘
                              │
                    ┌─────────▼─────────┐
                    │  VoiceChat.Data   │
                    │   (EF Core +      │
                    │    SQLite)        │
                    └───────────────────┘
```

## Technologie-Stack

### Backend

| Komponente | Technologie |
|------------|-------------|
| Framework | ASP.NET Core 10.0 |
| Echtzeit | SignalR |
| Audio | WebSocket + Opus (Concentus) |
| Authentifizierung | JWT Bearer Tokens |
| Datenbank | Entity Framework Core + SQLite |

### Frontend

| Komponente | Technologie |
|------------|-------------|
| Desktop | Avalonia 11.x |
| Mobile | .NET MAUI |
| Browser | WebAssembly |
| MVVM | CommunityToolkit.Mvvm |
| Audio | ManagedBass, RNNoise |

## API-Endpunkte

### REST API

| Methode | Pfad | Beschreibung |
|---------|------|-------------|
| `POST` | `/api/Login` | Anmelden und JWT erhalten |
| `POST` | `/api/validate` | JWT validieren |
| `POST` | `/api/GetChannels` | Alle Channels abrufen |

### SignalR Hub (`/connection`)

| Methode | Beschreibung |
|---------|-------------|
| `JoinChannel(channelId)` | Channel beitreten |
| `AddChannel(name)` | Channel erstellen |
| `DeleteChannel(channelId)` | Channel löschen |
| `SendMessage(message)` | Nachricht senden |
| `GetMessages(channelId, skip, take)` | Nachrichten laden |
| `KickUser(channelId, userId)` | Benutzer kicken |
| `BanUser(channelId, userId)` | Benutzer bannen |

### WebSocket Audio (`/audio`)

Binäres Audio-Streaming mit Opus-kodierten Daten.

**Query-Parameter:**
- `channelId`: Channel-ID
- `token`: JWT-Token

**Datenformat:**
```
[4 bytes: sequence number][variable: opus audio data]
```

## Projektstruktur

```
VoiceChat.slnx
├── VoiceChat.Api/           # Web API
├── VoiceChat.Data/          # Datenbank-Zugriff
├── VoiceChat.Entities/      # Domänen-Entitäten
├── VoiceChat.Shared/       # Geteilte DTOs
└── VoiceChat.Client/       # UI-Anwendungen
    ├── VoiceChat.Client/         # Geteilter UI-Code
    ├── VoiceChat.Client.Desktop/ # Desktop (Windows/Linux)
    ├── VoiceChat.Client.Browser/ # Web (WASM)
    ├── VoiceChat.Client.Android/ # Android
    └── VoiceChat.Client.iOS/     # iOS
```

## Datenmodell

### User
```csharp
public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public DateTime LastActive { get; set; }
    public Guid? ConnectedChannel { get; set; }
}
```

### Channel
```csharp
public class Channel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<User> Users { get; set; }
}
```

### Message
```csharp
public class Message
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ChannelId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

## Sicherheit

- **Authentifizierung**: JWT-Token mit Ablaufzeit
- **Validierung**: Serverseitige Eingabevalidierung
- **SQL Injection**: Geschützt durch EF Core
- **XSS**: Keine Benutzer-Eingaben werden als HTML gerendert

---

[← Zurück: Funktionen](./features)
