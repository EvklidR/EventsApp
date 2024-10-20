using EventsApp.EventsService.Domain.Entities;
using EventsApp.EventsService.Domain.Interfaces;
using EventsApp.EventsService.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventsApp.EventsService.Infrastructure.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly ApplicationDbContext _context;

        public ParticipantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ParticipantOfEvent participant)
        {
            await _context.Participants.AddAsync(participant);
        }

        public async Task DeleteAsync(int eventId, int userId)
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(p => p.EventId == eventId && p.UserId == userId);

            if (participant != null)
            {
                _context.Participants.Remove(participant);
            }
        }
        public async Task<ParticipantOfEvent> GetByIdAsync(int participantId)
        {
            return await _context.Participants.FindAsync(participantId);
        }

        public async Task<IEnumerable<ParticipantOfEvent>> GetByEventIdAsync(int eventId)
        {
            return await _context.Participants
                .Where(p => p.EventId == eventId)
                .ToListAsync();
        }
    }
}