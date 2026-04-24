using Microsoft.EntityFrameworkCore;
using ShiBoo.Models;
using System;
using System.IO;

namespace ShiBoo.Data
{
    public class ShiBooDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Shift> Shifts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "ShiBoo.db");
            options.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            
            modelBuilder.Entity<User>().HasData(new User 
            { 
                Id = 1, 
                Name = "System Admin", 
                Email = "admin@shiboo.com", 
                Password = "admin", 
                Role = "Admin", 
                IsFirstLogin = false 
            });
        }
    }
}