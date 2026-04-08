---
title: Server
lang: en
layout: default
previous: installation
previous_text: Installation
next: getting-started
next_text: Getting Started
---

# Server

## Setting Up the Server

### Prerequisites
- .NET 10.0 SDK installed
- Ports 5000 and 5001 available

### Start the Server

**Option 1: From Source**

```bash
# Clone repository
git clone https://github.com/KeanuDale-No1/Qwatschy.git
cd Qwatschy

# Start server
dotnet run --project VoiceChat.Api/VoiceChat.Api.csproj
```

**Option 2: Pre-built Server (Download)**

Download the server for your operating system:

| Platform | Download |
|----------|----------|
| Windows | [Qwatschy-Server-win.zip](https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-Server-win.zip) |
| Linux | [Qwatschy-Server-linux.zip](https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-Server-linux.zip) |

```bash
# Extract and run (Linux)
unzip Qwatschy-Server-linux.zip
./Qwatschy/Qwatschy
```

The server starts on `http://localhost:5000` by default.

## Server Configuration

API configuration is in `VoiceChat.Api/appsettings.json`:

```json
{
  "Urls": "http://0.0.0.0:5000",
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "VoiceChat",
    "Audience": "VoiceChat"
  }
}
```

### Important Settings

| Setting | Default | Description |
|---------|---------|-------------|
| `Urls` | `http://0.0.0.0:5000` | Server URL |
| `Jwt:Key` | (empty) | Secret key for JWT |
| `Jwt:ExpirationMinutes` | 60 | Token lifetime |

### Security

- **JWT Key**: Change the default key in production!
- **Firewall**: Open port 5000 for external connections
- **HTTPS**: A reverse proxy with HTTPS is recommended for production

## Server Commands

| Command | Description |
|---------|-------------|
| `dotnet run` | Start server |
| `dotnet run --urls "http://*:5000"` | Listen on all interfaces |
| `Ctrl+C` | Stop server |
