# Screenshots

Dieser Ordner enthält Screenshots für die Dokumentation.

## Benötigte Screenshots

| Dateiname | Beschreibung |
|-----------|-------------|
| `login.png` | Login-Bildschirm mit Username/Server-Eingabe |
| `channels.png` | Channel-Liste mit Benutzerliste |
| `create-channel.png` | Dialog zum Erstellen eines Channels |
| `chat.png` | Chat-Ansicht mit Nachrichten |
| `voice-chat.png` | Voice-Chat aktiv mit Mikrofon-Status |

## Wie Screenshots erstellen

1. **App starten:**
   ```bash
   # API starten
   dotnet run --project VoiceChat.Api/VoiceChat.Api.csproj
   
   # Client starten
   dotnet run --project VoiceChat.Client/VoiceChat.Client.Desktop/VoiceChat.Client.Desktop.csproj
   ```

2. **Screenshots machen:**
   - **Windows**: `Win + Shift + S` oder Snipping Tool
   - **macOS**: `Cmd + Shift + 4`
   - **Linux**: `PrtSc` oder Flameshot

3. **Bilder speichern:**
   - Format: PNG oder JPG
   - Empfohlene Größe: 800-1200px Breite
   - In diesem Ordner (`docs/assets/screenshots/`) speichern

4. **Bilder umbenennen:**
   - `login.png`
   - `channels.png`
   - `create-channel.png`
   - `chat.png`
   - `voice-chat.png`

## Anforderungen an Screenshots

- **Hell/Dunkel-Modus**: Screenshots im Hell-Modus bevorzugt
- **Keine persönlichen Daten**: Keine echten Usernamen oder Server-Adressen
- **Fokus auf Features**: Hauptfunktionen清晰的 zeigen
