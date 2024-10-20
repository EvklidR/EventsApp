using EventsApp.EventsService.Application.DTOs;


namespace EventsApp.EventsService.Application.Interfaces
{
    public interface IEventService
    {
        IEnumerable<EventDto> GetFilteredEvents(EventFilterDto filterDto);
        EventDto GetEventByIdAsync(int id);
        EventDto GetEventByNameAsync(string name);
        Task<EventDto> CreateEventAsync(CreateEventDto eventDto, IFormFile imageFile);
        Task UpdateEventAsync(UpdateEventDto updateEventDto);
        Task DeleteEventAsync(int id);
    }
}