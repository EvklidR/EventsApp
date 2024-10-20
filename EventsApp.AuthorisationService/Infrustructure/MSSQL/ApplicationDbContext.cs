using EventsApp.AuthorisationService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using EventsApp.AuthorisationService.Domain.Entities;

namespace EventsApp.AuthorisationService.Infrastructure.MSSQL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}