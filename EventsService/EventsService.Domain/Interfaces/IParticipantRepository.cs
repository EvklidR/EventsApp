using EventsService.Domain.Entities;
using System.Linq.Expressions;

namespace EventsService.Domain.Interfaces
{
    public interface IParticipantRepository
    {
        void Add(ParticipantOfEvent participant);
        void Delete(ParticipantOfEvent participantToDelete);
        Task<IEnumerable<ParticipantOfEvent>?> GetAsync(Expression<Func<ParticipantOfEvent, bool>> predicate);
    }
}
