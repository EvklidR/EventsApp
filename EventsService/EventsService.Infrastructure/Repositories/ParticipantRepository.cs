using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Infrastructure.Repositories
{
    public class ParticipantRepository : BaseRepository<ParticipantOfEvent>, IParticipantRepository
    {
        public ParticipantRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}