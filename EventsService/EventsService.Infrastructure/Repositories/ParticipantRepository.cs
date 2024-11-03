using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ParticipantOfEvent>?> GetAsync(Expression<Func<ParticipantOfEvent, bool>> predicate)
        {
            return await _context.Participants.Where(predicate).ToListAsync();
        }

    }
}