using System.Text.Json;
using VoiceChat.Shared.Models;

public class ChannelsService
{
    readonly string _filePath;
    readonly object _lock = new object();
    List<Channel> _channels = new List<Channel>();
    readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };

    public ChannelsService()
    {
        _filePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "channels.json");
        Load();
    }

    void Load()
    {
        try
        {
            if (System.IO.File.Exists(_filePath))
            {
                var json = System.IO.File.ReadAllText(_filePath);
                var list = JsonSerializer.Deserialize<List<Channel>>(json, _jsonOptions);
                if (list != null) _channels = list;
            }
        }
        catch
        {
            // ignore corrupt file
            _channels = new List<Channel>();
        }
    }

    Task SaveAsync()
    {
        return Task.Run(() =>
        {
            lock (_lock)
            {
                var json = JsonSerializer.Serialize(_channels, _jsonOptions);
                var tmp = _filePath + ".tmp";
                System.IO.File.WriteAllText(tmp, json);
                System.IO.File.Copy(tmp, _filePath, overwrite: true);
                try { System.IO.File.Delete(tmp); } catch { }
            }
        });
    }

    public Task<List<Channel>> GetAllAsync()
    {
        lock (_lock)
        {
            // return a copy
            return Task.FromResult(_channels.Select(c => new Channel { Id = c.Id, Name = c.Name, Description = c.Description }).ToList());
        }
    }

    public async Task<Channel> AddAsync(Channel channel)
    {
        lock (_lock)
        {
            if (channel.Id == Guid.Empty)
            {
                channel.Id = Guid.NewGuid();
            }
            _channels.Add(channel);
        }

        await SaveAsync();
        return channel;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var removed = false;
        lock (_lock)
        {
            var idx = _channels.FindIndex(c => c.Id == id);
            if (idx >= 0)
            {
                _channels.RemoveAt(idx);
                removed = true;
            }
        }

        if (removed)
        {
            await SaveAsync();
        }

        return removed;
    }
}
