using EventsService.Domain.Entities;
using System.Linq.Expressions;

namespace EventsService.Domain.Interfaces
{
    public interface IParticipantRepository
    {
        IQueryable<ParticipantOfEvent> GetAll();
        void Add(ParticipantOfEvent participant);
        void Delete(ParticipantOfEvent participantToDelete);
    }
}
