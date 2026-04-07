---
title: Architecture
lang: en
layout: default
previous: ./features
previous_text: Features
---

# Architecture

## Overview

Qwatschy is a modern, cross-platform application with a client-server architecture.

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

### WebSocket Audio (`/audio`)

Binary audio streaming with Opus-encoded data.

## Project Structure

```
VoiceChat.slnx
├── VoiceChat.Api/           # Web API
├── VoiceChat.Data/          # Database access
├── VoiceChat.Entities/      # Domain entities
├── VoiceChat.Shared/       # Shared DTOs
└── VoiceChat.Client/       # UI applications
    ├── VoiceChat.Client/         # Shared UI code
    ├── VoiceChat.Client.Desktop/ # Desktop
    ├── VoiceChat.Client.Browser/ # Web (WASM)
    ├── VoiceChat.Client.Android/ # Android
    └── VoiceChat.Client.iOS/     # iOS
```

## Security

- **Authentication**: JWT tokens with expiration
- **Validation**: Server-side input validation
- **SQL Injection**: Protected by EF Core
