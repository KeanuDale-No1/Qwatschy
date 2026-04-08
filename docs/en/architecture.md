---
title: Architecture
lang: en
layout: default
previous: features
previous_text: Features
---

# Architecture

## Overview

Qwatschy is a modern, cross-platform application with a client-server architecture.

![Architecture Diagram](../assets/images/architecture.svg)

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

## CI/CD Pipeline

### Build & Release Pipeline

The automated build pipeline runs on every push to the `master` branch:

| Job | Description | Outputs |
|-----|-------------|---------|
| `build-client` | Builds client for Windows & Linux | Velopack packages (.nupkg, AppImage, Setup.exe) |
| `build-deb` | Creates Debian package | .deb file |
| `build-flatpak` | Creates Flatpak package | .flatpak file |
| `build-server` | Builds server for Windows & Linux | ZIP archives |
| `release` | Creates GitHub Release | Tagged release with all artifacts |

**Versioning**: Base version from `VERSION` file + GitHub run number (e.g., `1.0.0.123`)

### Documentation Pipeline

The documentation is deployed to GitHub Pages automatically when:
- Files in `docs/` directory change
- The workflow file itself changes
- Manual trigger via `workflow_dispatch`

### Workflow Files

| File | Purpose |
|------|---------|
| `.github/workflows/main.yml` | Build & Release |
| `.github/workflows/deploy-docs.yml` | Documentation deployment |
