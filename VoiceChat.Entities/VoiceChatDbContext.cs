using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Entities.Auth;

namespace VoiceChat.Entities;

public class VoiceChatDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite("Data Source=voicechat.db");
}
