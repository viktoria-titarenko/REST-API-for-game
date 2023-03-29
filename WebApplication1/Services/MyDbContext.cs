using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public sealed class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Keys> Keys { get; set; }
        public DbSet<ProcessGame> ProcessGame { get; set; }
        public DbSet<Winners> Winners { get; set; }
    }
}