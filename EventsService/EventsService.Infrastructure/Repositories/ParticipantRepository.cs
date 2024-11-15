using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;

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
        public void Update(ParticipantOfEvent updatedParticipant)
        {
            _context.Participants.Update(updatedParticipant);
        }

        public async Task<IEnumerable<ParticipantOfEvent>> GetAllAsync()
        {
            return await _context.Participants.ToListAsync();
        }

        public async Task<ParticipantOfEvent?> GetByIdAsync(int id)
        {
            return await _context.Participants.FirstOrDefaultAsync(p => p.Id == id);
        }


    }
}