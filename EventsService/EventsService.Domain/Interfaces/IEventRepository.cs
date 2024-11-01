using EventsApp.EventsService.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EventsApp.EventsService.Domain.Interfaces
{
    public interface IEventRepository
    {
        IQueryable<Event> GetAll();
        Task AddAsync(Event newEvent);
        void Update(Event updatedEvent);
        Task DeleteAsync(int id);
        Task<string?> SaveImageAsync(IFormFile imageFile);
        void DeleteImage(string fileName);
    }
}
