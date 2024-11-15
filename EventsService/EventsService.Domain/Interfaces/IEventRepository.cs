using EventsService.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EventsService.Domain.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<Event?> GetByNameAsync(string name);
    }
}
