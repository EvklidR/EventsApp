using EventsService.Application.DTOs;
using EventsService.Domain.Entities;
using EventsService.Domain.Enums;
using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Infrastructure.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context)
        { }
        

        public override async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _dbSet.Include(e => e.Participants).ToListAsync();
        }

        public override async Task<Event?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event?> GetByNameAsync(string name)
        {
            return await _dbSet.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<IEnumerable<Event>?> GetPaginatedAsync(
            int pageNumber,
            int pageSize,
            DateTime? date = null,
            string? location = null,
            CategoryOfEvent? category = null)
        {
            IQueryable<Event> query = _dbSet.Include(e => e.Participants);

            if (date.HasValue)
            {
                query = query.Where(e => e.DateTimeHolding.Date == date.Value.Date);
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(e => e.Location.ToLower().Contains(location.ToLower()));
            }

            if (category.HasValue)
            {
                query = query.Where(e => e.Category == category.Value);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
