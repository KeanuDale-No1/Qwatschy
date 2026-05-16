using System;
using System.IO;
using System.Threading.Tasks;
using VoiceChat.Client.Services.AppSettings;

namespace VoiceChat.Client.Desktop.Services.AppSettings;

internal class FileStorageService : IStorageService
{
    private readonly string _filePath;

    public FileStorageService()
    {
        _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            QwatschyConstants.AppName,
            "settings.dat");
    }

    public string? Load()
    {
        if (!File.Exists(_filePath))
            return null;

        return File.ReadAllText(_filePath);
    }

    public async Task SaveAsync(string json)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        await File.WriteAllTextAsync(_filePath, json);
    }
}