using EventsApp.EventsService.Application.DTOs;
using EventsApp.EventsService.Domain.Entities;
using EventsApp.EventsService.Domain.Interfaces;
using EventsApp.EventsService.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.EventsService.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _imagePath;

        public EventRepository(ApplicationDbContext context, string imagePath)
        {
            _context = context;
            _imagePath = imagePath;
        }

        public async Task AddAsync(Event newEvent)
        {
            await _context.Events.AddAsync(newEvent);
        }

        public async Task DeleteAsync(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
            }
        }

        public IQueryable<Event> GetAll()
        {
            return _context.Events.Include(e => e.Participants).AsQueryable();
        }

        public async Task<string?> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(_imagePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return fileName;
            }

            return null;
        }

        public void Update(Event updatedEvent)
        {
            _context.Events.Update(updatedEvent);
        }

        public void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}