---
title: Architecture
lang: en
---

# Architecture

## Overview

Qwatschy is a modern, cross-platform application with a client-server architecture.

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

## Technology Stack

### Backend

| Component | Technology |
|-----------|------------|
| Framework | ASP.NET Core 10.0 |
| Real-time | SignalR |
| Audio | WebSocket + Opus (Concentus) |
| Authentication | JWT Bearer Tokens |
| Database | Entity Framework Core + SQLite |

### Frontend

| Component | Technology |
|-----------|------------|
| Desktop | Avalonia 11.x |
| Mobile | .NET MAUI |
| Browser | WebAssembly |
| MVVM | CommunityToolkit.Mvvm |
| Audio | ManagedBass, RNNoise |

## API Endpoints

### REST API

| Method | Path | Description |
|--------|------|-------------|
| `POST` | `/api/Login` | Login and get JWT |
| `POST` | `/api/validate` | Validate JWT |
| `POST` | `/api/GetChannels` | Get all channels |

### SignalR Hub (`/connection`)

| Method | Description |
|--------|-------------|
| `JoinChannel(channelId)` | Join a channel |
| `AddChannel(name)` | Create a channel |
| `DeleteChannel(channelId)` | Delete a channel |
| `SendMessage(message)` | Send a message |
| `GetMessages(channelId, skip, take)` | Load messages |
| `KickUser(channelId, userId)` | Kick user |
| `BanUser(channelId, userId)` | Ban user |

### WebSocket Audio (`/audio`)

Binary audio streaming with Opus-encoded data.

**Query Parameters:**
- `channelId`: Channel ID
- `token`: JWT token

**Data Format:**
```
[4 bytes: sequence number][variable: opus audio data]
```

## Project Structure

```
VoiceChat.slnx
├── VoiceChat.Api/           # Web API
├── VoiceChat.Data/          # Database access
├── VoiceChat.Entities/      # Domain entities
├── VoiceChat.Shared/       # Shared DTOs
└── VoiceChat.Client/       # UI applications
    ├── VoiceChat.Client/         # Shared UI code
    ├── VoiceChat.Client.Desktop/ # Desktop (Windows/Linux)
    ├── VoiceChat.Client.Browser/ # Web (WASM)
    ├── VoiceChat.Client.Android/ # Android
    └── VoiceChat.Client.iOS/     # iOS
```

## Data Model

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

## Security

- **Authentication**: JWT tokens with expiration
- **Validation**: Server-side input validation
- **SQL Injection**: Protected by EF Core
- **XSS**: No user input is rendered as HTML

---

[← Back: Features](./features)
