using EventsService.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EventsService.Domain.Interfaces
{
    public interface IEventRepository
    {
        IQueryable<Event> GetAll();
        void Add(Event newEvent);
        void Update(Event updatedEvent);
        void Delete(Event eventToDelete);

    }
}
