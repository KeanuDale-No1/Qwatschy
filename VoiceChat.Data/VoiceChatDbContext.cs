using Microsoft.EntityFrameworkCore;

namespace VoiceChat.Data;

public class VoiceChatDbContext : DbContext
{
    public DbSet<Domain.Auth.User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite("Data Source=voicechat.db");
}
