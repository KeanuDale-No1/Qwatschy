---
title: Installation
lang: de
layout: default
next: ./getting-started
next_text: Erste Schritte
---

# Installation

## Downloads

Lade den passenden Installer für dein Betriebssystem herunter:

| Plattform | Installer | Download |
|-----------|-----------|----------|
| Windows | `.exe` / `.msi` | [GitHub Releases](https://github.com/KeanuDale-No1/Qwatschy/releases) |
| macOS | `.dmg` | [GitHub Releases](https://github.com/KeanuDale-No1/Qwatschy/releases) |
| Linux | `.deb` / `.AppImage` | [GitHub Releases](https://github.com/KeanuDale-No1/Qwatschy/releases) |
| Android | `.apk` | [GitHub Releases](https://github.com/KeanuDale-No1/Qwatschy/releases) |
| iOS | TestFlight | <span class="badge badge-soon">Bald</span> |
| Web | Browser | [Web App](https://keanudale-no1.github.io/Qwatschy) |

## Windows

### Installation per Installer
1. Lade die `.exe` oder `.msi` Datei herunter
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
4. Öffne die App aus Applications

### Mikrofon-Erlaubnis
Bei der ersten Verwendung musst du in **System Settings → Privacy & Security → Microphone** die Erlaubnis erteilen.

## Linux

### Option A: AppImage
```bash
# Herunterladen
wget https://github.com/KeanuDale-No1/Qwatschy/releases/latest/Qwatschy.AppImage

# Ausführbar machen
chmod +x Qwatschy.AppImage

# Starten
./Qwatschy.AppImage
```

### Option B: DEB-Paket
```bash
# Herunterladen
wget https://github.com/KeanuDale-No1/Qwatschy/releases/latest/qwatschy.deb

# Installieren
sudo dpkg -i qwatschy.deb
sudo apt install -f

# Starten
qwatschy
```

### Audio-Abhängigkeiten
Falls Audio-Probleme auftreten:
```bash
sudo apt install libasound2 libbass
```

## Android

### Installation
1. Lade die `.apk` Datei herunter
2. Öffne die Datei
3. Falls nötig: Aktiviere "Aus unbekannten Quellen installieren" in den Einstellungen
4. Tippe auf "Installieren"
5. Öffne Qwatschy und erlaube den Mikrofon-Zugriff

### Anforderungen
- Android 8.0 oder höher
- Mikrofon-Erlaubnis

## iOS

Die iOS-Version wird über TestFlight verteilt. Sobald die App im App Store verfügbar ist, findest du hier den Link.

## Web (Browser)

Keine Installation nötig!

1. Öffne [https://keanudale-no1.github.io/Qwatschy](https://keanudale-no1.github.io/Qwatschy) in deinem Browser
2. Erlaube den Mikrofon-Zugriff wenn gefragt
3. Verbinde dich mit einem Server

### Unterstützte Browser
- Google Chrome / Chromium
- Mozilla Firefox
- Microsoft Edge
- Safari
