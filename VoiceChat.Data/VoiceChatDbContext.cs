using Microsoft.EntityFrameworkCore;

namespace VoiceChat.Data;

public class VoiceChatDbContext : DbContext
{
    public DbSet<Domain.Auth.User> Users { get; set; }
    public DbSet<Domain.Channel.Channel> Channels { get; set; }
    public DbSet<Domain.Channel.Message> Messages { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite("Data Source=voicechat.db");
}
