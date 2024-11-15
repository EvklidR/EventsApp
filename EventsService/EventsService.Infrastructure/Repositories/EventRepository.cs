using EventsService.Application.DTOs;
using EventsService.Domain.Entities;
using EventsService.Domain.Interfaces;
using EventsService.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Event newEvent)
        {
            _context.Events.AddAsync(newEvent);
        }

        public void Update(Event updatedEvent)
        {
            _context.Events.Update(updatedEvent);
        }

        public void Delete(Event eventToDelete)
        {
            _context.Events.Remove(eventToDelete);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event?> GetByNameAsync(string name)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.Name == name);
        }

    }
}