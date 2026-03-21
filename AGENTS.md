# VoiceChat Agent Guidelines

## Build Commands

### Build the Solution
```bash
dotnet build
```

### Build a Specific Project
```bash
dotnet build VoiceChat.Api/VoiceChat.Api.csproj
dotnet build VoiceChat.Client/VoiceChat.Client/VoiceChat.Client.csproj
dotnet build VoiceChat.Desktop/VoiceChat.Desktop.csproj
```

### Run the API
```bash
dotnet run --project VoiceChat.Api/VoiceChat.Api.csproj
```

### Run a Specific Client
```bash
dotnet run --project VoiceChat.Client/VoiceChat.Client/VoiceChat.Client.csproj
dotnet run --project VoiceChat.Desktop/VoiceChat.Desktop.csproj
```

### Run Tests (if tests exist)
```bash
dotnet test
dotnet test --filter "FullyQualifiedName~TestClassName"
dotnet test --filter "FullyQualifiedName~TestMethodName"
```

### Entity Framework Migrations
```bash
dotnet ef migrations add <MigrationName> --project VoiceChat.Data
dotnet ef database update --project VoiceChat.Data
```

### Clean and Rebuild
```bash
dotnet clean
dotnet build
```

---

## Code Style Guidelines

### General
- **Target Framework**: .NET 10.0
- **Nullable**: Enabled on all projects (`<Nullable>enable</Nullable>`)
- **Implicit Usings**: Enabled on all projects
- **LangVersion**: Latest (C# 12+) - Use primary constructors where appropriate

### Project Structure

```
VoiceChat.slnx
├── VoiceChat.Api/           # ASP.NET Core Web API with SignalR
├── VoiceChat.Data/          # EF Core + SQLite, repositories
├── VoiceChat.Entities/      # Domain entities
├── VoiceChat.Shared/        # Shared DTOs, interfaces, models
├── VoiceChat.Client/        # Avalonia desktop client
└── VoiceChat.Desktop/       # MAUI cross-platform client
```

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `User`, `Channel` |
| Interfaces | PascalCase with `I` prefix | `IUseCase`, `IRepository` |
| Methods | PascalCase | `ExecuteAsync`, `GetByIdAsync` |
| Properties | PascalCase | `UserId`, `DisplayName` |
| Private fields | `_camelCase` | `_context`, `_secretKey` |
| Local variables | camelCase | `userId`, `loginRequest` |
| Parameters | camelCase | `request`, `userId` |
| Namespaces | PascalCase | `VoiceChat.Api.UseCases` |
| DTOs | PascalCase with `DTO` suffix | `LoginRequestDTO` |
| Records | PascalCase with `DTO` suffix | `LoginResponseDTO(Guid UserId, ...)` |
| Enums | PascalCase | `UserStatus` |

### Primary Constructors (C# 12+)
Use primary constructors for services and use cases:
```csharp
public class LoginUsecase(IRepository<User> repository, IAuthService authService) 
    : IUseCase<LoginRequestDTO, LoginResponseDTO>
{
    // Use repository, authService directly
}
```

### Imports/Using Statements
- Group by type: `System` first, then third-party (`Microsoft`, `CommunityToolkit`), then project (`VoiceChat`)
- Remove unused imports
- Use file-scoped namespaces (no braces around namespace)

### Formatting
- Indent with 4 spaces
- Use expression-bodied members where concise
- Use `var` for local variables when type is obvious
- Keep lines under 120 characters when reasonable
- No spaces before/after generic brackets: `List<string>`, not `List< string >`

### Type Guidelines
- Use `string` instead of `String`
- Use `Task` for async return types
- Use `Guid` instead of `Guid.Empty` checks
- Use nullable reference types (`string?`) for optional values
- Use records for immutable DTOs
- Use `List<T>` for collections, initialize with `new List<T>()` or `new()` (target-typed new)

### Error Handling
- Use specific exceptions: `ArgumentNullException`, `ArgumentException`, `UnauthorizedAccessException`
- For input validation: throw with descriptive message
- In endpoints: catch exceptions and return appropriate `Results` response
- Never expose internal errors to clients; log internally

### Async/Await
- Always use `Async` suffix for async methods: `GetByIdAsync`
- Use `await` over `.Result` or `.GetAwaiter().GetResult()`
- Return `Task` or `Task<T>`, never `void` (except event handlers)

### Entity Framework
- Entities inherit from `EntityBase` which provides `Id` and `CreatedAt`
- Use repository pattern: `IRepository<T>` with `Repository<T>` implementation
- Use `async` methods for all DB operations
- Call `SaveAsync()` after multiple changes

### SignalR Hubs
- Use SignalR for real-time communication (audio, chat)
- Hub methods are invoked by clients
- Use `Clients.Group()` for channel-specific messaging

### Dependency Injection
- Register services in `Program.cs` or extension methods
- Use `AddScoped` for services with per-request lifetime
- Use `AddSingleton` for shared state services
- Use `AddTransient` for lightweight stateless services

### Client-Side (Avalonia/MAUI)
- Use MVVM with `CommunityToolkit.Mvvm`
- Base ViewModels inherit from `ViewModelBase : ObservableObject`
- Use `[ObservableProperty]` and `[RelayCommand]` source generators
- ViewModels injected via DI constructor

### Comments
- Use comments to explain **why**, not **what**
- German comments may appear in some legacy code
- Document public API surfaces (interfaces, base classes)

---

## Troubleshooting

### Linux Audio Dependencies
```bash
sudo apt install libasound2
```

### Database Issues
Delete `voicechat.db` and run migrations to recreate:
```bash
rm voicechat.db
dotnet ef database update --project VoiceChat.Data
```
