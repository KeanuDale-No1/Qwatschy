---
title: Installation
lang: en
---

# Installation

## Downloads

Download the appropriate installer for your operating system:

| Platform | Installer | Download |
|----------|-----------|----------|
| Windows | `.exe` / `.msi` | [GitHub Releases](https://github.com/YOUR_USERNAME/VoiceChat/releases) |
| macOS | `.dmg` | [GitHub Releases](https://github.com/YOUR_USERNAME/VoiceChat/releases) |
| Linux | `.deb` / `.AppImage` | [GitHub Releases](https://github.com/YOUR_USERNAME/VoiceChat/releases) |
| Android | `.apk` | [GitHub Releases](https://github.com/YOUR_USERNAME/VoiceChat/releases) |
| iOS | TestFlight | Coming soon |
| Web | Browser | [Web App](https://YOUR_USERNAME.github.io/VoiceChat) |

## Windows

### Installer Installation
1. Download the `.exe` or `.msi` file
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
4. Open the app from Applications

### Microphone Permission
On first use, grant permission in **System Settings → Privacy & Security → Microphone**.

## Linux

### Option A: AppImage
```bash
# Download
wget https://github.com/YOUR_USERNAME/VoiceChat/releases/latest/Qwatschy.AppImage

# Make executable
chmod +x Qwatschy.AppImage

# Run
./Qwatschy.AppImage
```

### Option B: DEB Package
```bash
# Download
wget https://github.com/YOUR_USERNAME/VoiceChat/releases/latest/qwatschy.deb

# Install
sudo dpkg -i qwatschy.deb
sudo apt install -f

# Run
qwatschy
```

### Audio Dependencies
If you experience audio issues:
```bash
sudo apt install libasound2 libbass
```

## Android

### Installation
1. Download the `.apk` file
2. Open the file
3. If needed: Enable "Install from unknown sources" in Settings
4. Tap "Install"
5. Open Qwatschy and grant microphone permission

### Requirements
- Android 8.0 or higher
- Microphone permission

## iOS

The iOS version is distributed via TestFlight. Once the app is available on the App Store, the link will be provided here.

## Web (Browser)

No installation required!

1. Open [https://YOUR_USERNAME.github.io/VoiceChat](https://YOUR_USERNAME.github.io/VoiceChat) in your browser
2. Grant microphone permission when prompted
3. Connect to a server

### Supported Browsers
- Google Chrome / Chromium
- Mozilla Firefox
- Microsoft Edge
- Safari

---

[← Back to Home](../) | [Next: Getting Started →](./getting-started)
