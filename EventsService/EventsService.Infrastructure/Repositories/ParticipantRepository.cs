using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.MSSQL;

namespace EventsService.Infrastructure.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly ApplicationDbContext _context;

        public ParticipantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ParticipantOfEvent participant)
        {
            _context.Participants.AddAsync(participant);
        }

        public void Delete(ParticipantOfEvent participantToDelete)
        {
            _context.Participants.Remove(participantToDelete);
        }

        public IQueryable<ParticipantOfEvent> GetAll()
        {
            return _context.Participants.AsQueryable();
        }

    }
}