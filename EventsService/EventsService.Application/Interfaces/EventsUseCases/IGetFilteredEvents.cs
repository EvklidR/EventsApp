using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface IGetFilteredEvents
    {
        IEnumerable<EventDto>? Execute(EventFilterDto filterDto);
    }
}
