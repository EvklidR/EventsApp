using EventsService.Domain.Entities;

namespace EventsService.Domain.Interfaces
{
    public interface IParticipantRepository
    {
        Task AddAsync(ParticipantOfEvent participant);
        Task DeleteAsync(int eventId, int userId);
        Task<ParticipantOfEvent> GetByIdAsync(int participantId);
        Task<IEnumerable<ParticipantOfEvent>> GetByEventIdAsync(int eventId);
    }
}
