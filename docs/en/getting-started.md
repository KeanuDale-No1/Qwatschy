---
title: Getting Started
lang: en
layout: default
previous: installation
previous_text: Installation
next: features
next_text: Features
---

# Getting Started

## Setting Up the Server

### Prerequisites
- .NET 10.0 SDK installed
- Ports 5000 and 5001 available

### Start the Server
```bash
cd VoiceChat.Api
dotnet run
```

The server starts on `http://localhost:5000` by default.

### Server Configuration

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

## Connecting to the Server

### Start the Client
1. Launch Qwatschy on your device
2. You'll see the login screen

<div class="screenshot-wrapper">
  <img src="../assets/screenshots/login.png" alt="Login Screen" class="screenshot">
</div>

### Login
1. Enter a **username** (e.g., "John")
2. Enter the **server address**:
   - Local: `http://localhost:5000`
   - Network: `http://192.168.x.x:5000`
   - Online: `https://your-server.com`
3. Click **Connect**

### Save Connection
Your recent connections are automatically saved and can be restored via the dropdown menu.

## Create Your First Channel

1. Click the **+** symbol in the channel bar
2. Enter a name for the channel (e.g., "General")
3. Click **Create**

<div class="screenshot-wrapper">
  <img src="../assets/screenshots/create-channel.png" alt="Create Channel" class="screenshot">
</div>

## Managing Users

### View Online Users
Online users are displayed in the left sidebar under the channel.

### Kick/Ban Users
Right-click on a user → Kick or Ban
