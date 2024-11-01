using EventsApp.EventsService.Domain.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace EventsApp.EventsService.Domain.Interfaces
{
    public interface IParticipantRepository
    {
        Task AddAsync(ParticipantOfEvent participant);
        Task DeleteAsync(int eventId, int userId);
        Task<ParticipantOfEvent> GetByIdAsync(int participantId);
        Task<IEnumerable<ParticipantOfEvent>> GetByEventIdAsync(int eventId);
    }
}
