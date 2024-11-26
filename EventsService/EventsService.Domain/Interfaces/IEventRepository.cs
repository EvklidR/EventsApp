using EventsService.Domain.Entities;
using EventsService.Domain.Enums;

namespace EventsService.Domain.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<Event?> GetByNameAsync(string name);
        Task<IEnumerable<Event>?> GetPaginatedAsync(
            int pageNumber,
            int pageSize,
            DateTime? date = null,
            string? location = null,
            CategoryOfEvent? category = null);
    }
}
