---
title: Installation
lang: en
layout: default
next: getting-started
next_text: Getting Started
---

# Installation

## Downloads

Download the installer for your operating system:

<div class="download-buttons">
  <a href="https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-win-Setup.exe" class="download-btn windows">
    <span class="icon">🪟</span>
    <span class="text">
      <strong>Windows</strong>
      <small>.exe Installer</small>
    </span>
  </a>
 
  <a href="https://keanudale-no1.github.io/Qwatschy" class="download-btn web">
    <span class="icon">🌐</span>
    <span class="text">
      <strong>Web</strong>
      <small>Open in browser</small>
    </span>
  </a>
</div>

<p class="text-center">
  <a href="https://github.com/KeanuDale-No1/Qwatschy/releases" class="all-versions">📋 View all versions</a>
</p>

## Windows

### Installation
1. Download the `.exe` file
2. Run the installer
3. Follow the on-screen instructions
4. Launch "Qwatschy" from the Start menu

### System Requirements
- Windows 10 or higher
- Microphone and speakers/headset
- Internet connection

## macOS

### Installation
1. Download the `.dmg` file
2. Open the `.dmg` file
3. Drag **Qwatschy** to the Applications folder
4. Open the app from Applications (confirm macOS warning)

### Microphone Permission
On first use, grant permission in **System Settings → Privacy & Security → Microphone**.

## Linux

Linux builds are coming soon! <span class="badge badge-soon">Soon</span>

Alternatively, build Qwatschy from source:
```bash
git clone https://github.com/KeanuDale-No1/Qwatschy.git
cd Qwatschy
dotnet build VoiceChat.slnx
dotnet run --project VoiceChat.Client/VoiceChat.Client.Desktop/VoiceChat.Client.Desktop.csproj
```

## Android

### Installation
1. Download the `.apk` file
2. Open the file
3. Enable "Install from unknown sources" if needed
4. Tap "Install"
5. Open Qwatschy and grant microphone permission

### Requirements
- Android 8.0 or higher

## Web (Browser)

No installation needed! Just open:

👉 [https://keanudale-no1.github.io/Qwatschy](https://keanudale-no1.github.io/Qwatschy)

### Supported Browsers
- Google Chrome / Chromium
- Mozilla Firefox
- Microsoft Edge
- Safari

## iOS

The iOS version is coming soon!
