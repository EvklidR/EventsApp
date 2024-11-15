using Microsoft.EntityFrameworkCore;
using EventsService.Domain.Entities;
using EventsService.Infrastructure.MSSQL.Configurations;

namespace EventsService.Infrastructure.MSSQL
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
            modelBuilder.ApplyConfiguration(new ParticipantOfEventConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
        }
    }
}
