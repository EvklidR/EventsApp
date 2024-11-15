using EventsService.Domain.Entities;
using System.Linq.Expressions;

namespace EventsService.Domain.Interfaces
{
    public interface IParticipantRepository : IBaseRepository<ParticipantOfEvent>;
}
