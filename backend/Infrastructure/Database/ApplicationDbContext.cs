using backend.Domain.Entities;
using BCrypt.Net;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Pessoa> Pessoas { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            if (Database.IsInMemory())
            {
                SeedInMemoryData();
            }
        }

        private void SeedInMemoryData()
        {
            if (!Users.Any())
            {
                Users.Add(new User
                {
                    Id = 1,
                    Email = "admin@teste.com",
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("admin123"),
                    Role = Role.Admin.ToString()
                });
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@teste.com",
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("admin123"),
                    Role = Role.Admin.ToString()
                }
            );

            modelBuilder.Entity<Pessoa>()
                .HasIndex(p => p.Cpf)
                .IsUnique();

            modelBuilder.Entity<Pessoa>()
                .HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}
