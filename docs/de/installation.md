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
  <a href="https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-Setup-win.exe" class="download-btn windows">
    <span class="icon">🪟</span>
    <span class="text">
      <strong>Windows</strong>
      <small>.exe Installer</small>
    </span>
  </a>
  <a href="https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-linux.nupkg" class="download-btn linux">
    <span class="icon">🐧</span>
    <span class="text">
      <strong>Linux</strong>
      <small>Velopack</small>
    </span>
  </a>
  <a href="https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-deb.deb" class="download-btn linux">
    <span class="icon">🐧</span>
    <span class="text">
      <strong>Linux (DEB)</strong>
      <small>.deb Paket</small>
    </span>
  </a>
  <a href="https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy.flatpak" class="download-btn linux">
    <span class="icon">🐧</span>
    <span class="text">
      <strong>Linux (Flatpak)</strong>
      <small>.flatpak Paket</small>
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

## Linux

### Option 1: Velopack (Empfohlen)

```bash
# Velopack CLI installieren
dotnet tool install -g vpk

# Qwatschy installieren
vpk install-from-package https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-linux.nupkg
```

### Option 2: DEB-Paket

```bash
# Herunterladen und installieren
wget https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-deb.deb
sudo dpkg -i Qwatschy-deb.deb
sudo apt install -f
```

### Option 3: Flatpak

```bash
# Flathub hinzufügen falls noch nicht vorhanden
flatpak remote-add --if-not-exists flathub https://flathub.org/repo/flathub.flatpakrepo

# Qwatschy installieren
flatpak install flathub com.qwatschy.Qwatschy

# Ausführen
flatpak run com.qwatschy.Qwatschy
```

### Option 4: Arch Linux (AUR)

```bash
# Mit einem AUR-Helfer wie yay
yay -S qwatschy
```

## Web (Browser)

Keine Installation nötig! Öffne einfach:

👉 [https://keanudale-no1.github.io/Qwatschy](https://keanudale-no1.github.io/Qwatschy)

### Unterstützte Browser
- Google Chrome / Chromium
- Mozilla Firefox
- Microsoft Edge
- Safari

## Server aufsetzen

Du möchtest einen eigenen Server betreiben? ➡️ [Server einrichten](./server.html)

## Weitere Plattformen

**macOS, Android und iOS** Builds sind in Entwicklung und werden in Kürze verfügbar sein!
