using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EventsService.Domain.Entities;

namespace EventsService.Infrastructure.MSSQL.Configurations
{
    public class ParticipantOfEventConfiguration : IEntityTypeConfiguration<ParticipantOfEvent>
    {
        public void Configure(EntityTypeBuilder<ParticipantOfEvent> builder)
        {
            builder.HasIndex(p => new { p.EventId, p.UserId })
                   .IsUnique();

            builder.HasOne<Event>()
                   .WithMany(e => e.Participants)
                   .HasForeignKey(p => p.EventId);
        }
    }
}
