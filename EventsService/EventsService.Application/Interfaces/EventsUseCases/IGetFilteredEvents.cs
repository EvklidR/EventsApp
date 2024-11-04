using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface IGetFilteredEvents
    {
        Task<IEnumerable<EventDto>?> ExecuteAsync(EventFilterDto filterDto);
    }
}
