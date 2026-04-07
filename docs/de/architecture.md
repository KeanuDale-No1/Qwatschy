---
title: Architektur
lang: de
layout: default
previous: features
previous_text: Funktionen
---

# Architektur

## Überblick

Qwatschy ist eine moderne, plattformübergreifende Anwendung mit einer Client-Server-Architektur.

![Architektur-Diagramm](../assets/images/architecture.svg)

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

### WebSocket Audio (`/audio`)

Binäres Audio-Streaming mit Opus-kodierten Daten.

## Projektstruktur

```
VoiceChat.slnx
├── VoiceChat.Api/           # Web API
├── VoiceChat.Data/          # Datenbank-Zugriff
├── VoiceChat.Entities/      # Domänen-Entitäten
├── VoiceChat.Shared/       # Geteilte DTOs
└── VoiceChat.Client/       # UI-Anwendungen
    ├── VoiceChat.Client/         # Geteilter UI-Code
    ├── VoiceChat.Client.Desktop/ # Desktop
    ├── VoiceChat.Client.Browser/ # Web (WASM)
    ├── VoiceChat.Client.Android/ # Android
    └── VoiceChat.Client.iOS/     # iOS
```

## Sicherheit

- **Authentifizierung**: JWT-Token mit Ablaufzeit
- **Validierung**: Serverseitige Eingabevalidierung
- **SQL Injection**: Geschützt durch EF Core
