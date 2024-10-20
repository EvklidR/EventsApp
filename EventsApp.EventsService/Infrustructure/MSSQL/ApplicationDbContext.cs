using Microsoft.EntityFrameworkCore;
using EventsApp.EventsService.Domain.Entities;

namespace EventsApp.EventsService.Infrastructure.MSSQL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<ParticipantOfEvent> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParticipantOfEvent>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ParticipantOfEvent>()
                .HasIndex(p => new { p.EventId, p.UserId })
                .IsUnique();

            modelBuilder.Entity<ParticipantOfEvent>()
                .HasOne<Event>()
                .WithMany(e => e.Participants)
                .HasForeignKey(p => p.EventId);

            modelBuilder.Entity<Event>()
                .HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}