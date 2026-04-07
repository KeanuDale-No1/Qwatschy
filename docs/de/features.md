---
title: Funktionen
lang: de
layout: default
previous: getting-started
previous_text: Erste Schritte
next: architecture
next_text: Architektur
---

# Funktionen

## Login & Authentifizierung

<div class="screenshot-wrapper">
  <img src="../assets/screenshots/login.png" alt="Login Screen" class="screenshot">
</div>

- **Benutzername**: Wähle einen eindeutigen Namen
- **Server-Adresse**: IP oder Domain des Servers
- **Verbindungshistorie**: Zuletzt genutzte Server werden gespeichert
- **JWT-Authentifizierung**: Sichere Anmeldung mit Token

## Channel-Verwaltung

<div class="screenshot-wrapper">
  <img src="../assets/screenshots/VoiceChat.png" alt="Channel List" class="screenshot">
</div>

### Channels erstellen
1. Klicke auf das **+** Symbol
2. Gib einen Channel-Namen ein
3. Bestätige mit Enter oder Klick auf "Erstellen"

<div class="screenshot-wrapper">
  <img src="../assets/screenshots/create-channel.png" alt="Channel erstellen" class="screenshot">
</div>

### Channels beitreten
- Klicke auf einen Channel in der Liste
- Der Channel wird automatisch geöffnet

### Channels löschen
- Rechtsklick auf Channel → "Löschen"
- Nur der Ersteller kann Channels löschen

## Text-Chat

<div class="screenshot-wrapper">
  <img src="../assets/screenshots/VoiceChat.png" alt="Chat View" class="screenshot">
</div>

### Nachrichten senden
1. Wähle einen Channel aus
2. Schreibe deine Nachricht in das Textfeld unten
3. Drücke **Enter** zum Senden

### Nachrichten laden
- Bei langen Gesprächen: Klicke auf "Mehr laden..." oben
- Nachrichten werden seitenweise geladen

### Ungelesene Nachrichten
- Channels mit neuen Nachrichten zeigen einen **Zähler** an
- Der Zähler erscheint links neben dem Channel-Namen

## Voice Chat

<div class="screenshot-wrapper">
  <img src="../assets/screenshots/join-voicechat.png" alt="Voice Chat" class="screenshot">
</div>

### Voice-Chat beitreten
1. Klicke auf den **Voice-Button** (🎤) eines Channels
2. Warte bis die Verbindung hergestellt ist
3. Dein Mikrofon ist jetzt aktiv

### Audio-Einstellungen
- **Stumm (Mute)**: Drücke **M** oder klicke auf das Mikrofon-Symbol
- **Ton an/aus**: Klicke auf den Lautsprecher-Button

### Audio-Technologie
- **Opus Codec**: Hochqualitative Audio-Kodierung
- **WebSocket**: Echtzeit-Audio-Streaming
- **Rauschunterdrückung**: RNNoise für sauberen Sound

## Benutzer-Verwaltung

### Online-Benutzer anzeigen
- Liste der aktuell verbundenen Benutzer im Channel
- Aktueller Benutzer wird hervorgehoben

### Benutzer-Aktionen (Rechtsklick)
- **Kicken**: Temporär aus dem Channel entfernen
- **Bannen**: Dauerhaft aus dem Channel ausschließen

## Design & Themes

### Hell-Modus
- Helles Farbschema mit Blau/Türkis-Akzenten
- Standardansicht

### Dunkel-Modus
- Dunkles Farbschema mit Pink-Akzenten (#e62f79)
- Augenfreundlich bei Nacht

## Tastenkürzel

| Taste | Aktion |
|-------|--------|
| **Enter** | Nachricht senden |
| **M** | Mikrofon stumm schalten |
| **Escape** | Abbrechen / Schließen |
