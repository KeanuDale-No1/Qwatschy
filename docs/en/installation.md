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
      <small>.deb package</small>
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

## Linux

### Option 1: Velopack (Recommended)

```bash
# Install Velopack CLI
dotnet tool install -g vpk

# Install Qwatschy
vpk install-from-package https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-linux.nupkg
```

### Option 2: DEB Package

```bash
# Download and install
wget https://github.com/KeanuDale-No1/Qwatschy/releases/latest/download/Qwatschy-deb.deb
sudo dpkg -i Qwatschy-deb.deb
sudo apt install -f
```

### Option 3: Arch Linux (AUR)

```bash
# Using an AUR helper like yay
yay -S qwatschy
```

## Web (Browser)

No installation needed! Just open:

👉 [https://keanudale-no1.github.io/Qwatschy](https://keanudale-no1.github.io/Qwatschy)

### Supported Browsers
- Google Chrome / Chromium
- Mozilla Firefox
- Microsoft Edge
- Safari

## Set Up a Server

Want to run your own server? ➡️ [Set up server](./server.html)

## Other Platforms

**macOS, Android, and iOS** builds are in development and will be available soon!
