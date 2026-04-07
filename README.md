# VoiceChat

A real-time voice and text chat application built with .NET 10 and Avalonia UI, supporting multiple platforms.

## Features

- **Real-time voice chat** with Opus codec encoding
- **Text messaging** in dedicated channels
- **Cross-platform** - Windows, Linux, Web (WASM), Android, iOS
- **JWT authentication** with auto-login
- **Noise suppression** using RNNoise
- **Modern MVVM architecture** with CommunityToolkit.Mvvm

## Architecture

```
VoiceChat.slnx
├── VoiceChat.Api/          # ASP.NET Core Web API + SignalR + WebSocket
├── VoiceChat.Data/         # Entity Framework Core + SQLite
├── VoiceChat.Entities/     # Domain entities
├── VoiceChat.Shared/       # Shared DTOs and interfaces
└── VoiceChat.Client/       # Avalonia cross-platform UI
    ├── VoiceChat.Client/         # Shared UI code
    ├── VoiceChat.Client.Desktop/ # Windows/Linux desktop
    ├── VoiceChat.Client.Browser/ # WebAssembly browser
    ├── VoiceChat.Client.Android/ # Android mobile
    └── VoiceChat.Client.iOS/     # iOS mobile
```

## Technologies

| Layer | Technology |
|-------|------------|
| Backend | ASP.NET Core 10, SignalR, WebSockets |
| Database | Entity Framework Core, SQLite |
| Auth | JWT Bearer Tokens |
| Audio | Opus Codec (Concentus), ManagedBass, RNNoise |
| Frontend | Avalonia 11.x, CommunityToolkit.Mvvm |

## Getting Started

### Prerequisites

- .NET 10.0 SDK
- For desktop audio: `libasound2` (Linux)

### Build

```bash
dotnet build
```

### Run API

```bash
dotnet run --project VoiceChat.Api/VoiceChat.Api.csproj
```

The API starts on `http://localhost:5000`.

### Run Clients

```bash
# Desktop (Windows/Linux)
dotnet run --project VoiceChat.Client/VoiceChat.Client.Desktop/VoiceChat.Client.Desktop.csproj

# Browser (WebAssembly)
dotnet run --project VoiceChat.Client/VoiceChat.Client.Browser/VoiceChat.Client.Browser.csproj

# Android
dotnet run --project VoiceChat.Client/VoiceChat.Client.Android/VoiceChat.Client.Android.csproj
```

### Database Migrations

```bash
dotnet ef migrations add <MigrationName> --project VoiceChat.Data
dotnet ef database update --project VoiceChat.Data
```

To reset the database, delete `voicechat.db` and run migrations.

## Configuration

### API (VoiceChat.Api/appsettings.json)

```json
{
  "JwtOptions": {
    "Issuer": "VoiceChat",
    "Audience": "VoiceChat",
    "Expires": 1,
    "SecretKey": "your-secret-key-minimum-32-characters"
  }
}
```

## API Reference

### REST Endpoints

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/login` | Login/register and receive JWT |
| POST | `/api/validate` | Validate JWT token |

### SignalR Hub (`/connection`)

| Method | Description |
|--------|-------------|
| `JoinChannel(channelId)` | Join a channel |
| `AddChannel(channel)` | Create a channel |
| `DeleteChannel(channelId)` | Delete a channel |
| `SendMessage(message)` | Send a chat message |
| `GetMessages(channelId, skip, take)` | Get paginated messages |

### WebSocket (`/audio`)

Binary audio streaming with query parameters `channelId` and `token`.

## Data Models

### Entities

- **User**: Id, Username, LastActive, ConnectedChannel
- **Channel**: Id, Name, Description, Messages, Users
- **Message**: Id, SenderId, ChannelId, Content, CreatedAt

