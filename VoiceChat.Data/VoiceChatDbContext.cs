using Microsoft.EntityFrameworkCore;

namespace VoiceChat.Data;

public class VoiceChatDbContext : DbContext
{

    protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite("Data Source=voicechat.db");
}
