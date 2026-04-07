# VoiceChat - Projektbeschreibung

## Überblick

VoiceChat ist eine Echtzeit-Sprach- und Text-Chat-Anwendung, die es Benutzern ermöglicht, sich mit Servern zu verbinden, Kanäle zu erstellen und in Echtzeit über Sprache und Text zu kommunizieren.

## Hauptfunktionen

- **Sprachkommunikation**: Echtzeit-Übertragung von Audio mit Opus-Codec
- **Text-Chat**: Nachrichten in dedizierten Kanälen
- **Kanalverwaltung**: Erstellen und Löschen von Kanälen
- **Authentifizierung**: JWT-basierte Anmeldung mit Auto-Login
- **Plattformübergreifend**: Windows, Linux, Browser, Android, iOS

## Frontend-Architektur

Das Frontend ist mit **Avalonia UI** und **MVVM-Pattern** aufgebaut:

```
VoiceChat.Client/
├── Services/
│   ├── ApiService.cs         # REST-API-Kommunikation
│   ├── NavigationService.cs  # View-Navigation
│   ├── SignalRService.cs     # Echtzeit-Messaging
│   └── VoiceService.cs       # Audio-Verbindungen
├── ViewModels/
│   ├── MainViewModel.cs      # Hauptansicht
│   ├── LoginViewModel.cs     # Anmeldung
│   ├── HomeViewModel.cs      # Startseite
│   ├── ChannelViewModel.cs   # Kanalansicht
│   └── VoiceChatViewModel.cs # Sprachchat
└── Views/
    ├── MainWindow.axaml
    ├── LoginView.axaml
    ├── HomeView.axaml
    └── VoiceChatView.axaml
```

### Schichten

1. **Views (AXAML)**: UI-Layouts mit XAML definiert
2. **ViewModels**: Logik und Zustandsverwaltung mit `[ObservableProperty]` und `[RelayCommand]`
3. **Services**: Kommunikation mit Backend (API, SignalR, WebSocket)

### SignalR-Client

Der SignalR-Client (`SignalRService.cs`) verwaltet:
- Verbindung zum Hub (`/connection`)
- Kanalbeitritt und -verlassen
- Nachrichten senden und empfangen
- Echtzeit-Updates für alle Clients

### Voice-Client

Der Voice-Client (`VoiceService.cs`) kümmert sich um:
- WebSocket-Verbindung (`/audio`)
- Audio-Capture und -Wiedergabe
- Opus-Encodierung und -Decodierung
- Geräuschunterdrückung (RNNoise)
