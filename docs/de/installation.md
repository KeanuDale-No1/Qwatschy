---
title: Installation
lang: de
layout: default
next: getting-started
next_text: Erste Schritte
---

# Installation

## Downloads

Lade den passenden Installer für dein Betriebssystem herunter:

<div class="download-buttons">
  <a href="https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-win-x64.exe" class="download-btn windows">
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
      <small>Im Browser öffnen</small>
    </span>
  </a>
</div>

<p class="text-center">
  <a href="https://github.com/KeanuDale-No1/Qwatschy/releases" class="all-versions">📋 Alle Versionen anzeigen</a>
</p>

## Windows

### Installation
1. Lade die `.exe` Datei herunter
2. Führe den Installer aus
3. Folgere den Anweisungen am Bildschirm
4. Starte "Qwatschy" aus dem Startmenü

### Systemanforderungen
- Windows 10 oder höher
- Mikrofon und Lautsprecher/Headset
- Internetverbindung

## macOS

### Installation
1. Lade die `.dmg` Datei herunter
2. Öffne die `.dmg` Datei
3. Ziehe **Qwatschy** in den Applications-Ordner
4. Öffne die App aus Applications (macOS Warnung bestätigen)

### Mikrofon-Erlaubnis
Bei der ersten Verwendung musst du in **System Settings → Privacy & Security → Microphone** die Erlaubnis erteilen.

## Linux

Linux-Builds werden in Kürze verfügbar sein! <span class="badge badge-soon">Bald</span>

Alternativ kannst du Qwatschy aus dem Quellcode bauen:
```bash
git clone https://github.com/KeanuDale-No1/Qwatschy.git
cd Qwatschy
dotnet build VoiceChat.slnx
dotnet run --project VoiceChat.Client/VoiceChat.Client.Desktop/VoiceChat.Client.Desktop.csproj
```

## Android

### Installation
1. Lade die `.apk` Datei herunter
2. Öffne die Datei
3. Aktiviere "Aus unbekannten Quellen installieren" falls nötig
4. Tippe auf "Installieren"
5. Öffne Qwatschy und erlaube den Mikrofon-Zugriff

### Anforderungen
- Android 8.0 oder höher

## Web (Browser)

Keine Installation nötig! Öffne einfach:

👉 [https://keanudale-no1.github.io/Qwatschy](https://keanudale-no1.github.io/Qwatschy)

### Unterstützte Browser
- Google Chrome / Chromium
- Mozilla Firefox
- Microsoft Edge
- Safari

## iOS

Die iOS-Version kommt in Kürze!
