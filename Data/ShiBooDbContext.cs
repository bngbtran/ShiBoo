using Microsoft.EntityFrameworkCore;
using ShiBoo.Models;

namespace ShiBoo.Data
{
    public class ShiBooDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Shift> Shifts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=ShiBoo.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            
            // Seed data cho Admin mặc định nếu DB trống
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