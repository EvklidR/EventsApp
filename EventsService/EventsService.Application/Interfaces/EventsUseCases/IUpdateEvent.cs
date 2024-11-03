using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface IUpdateEvent
    {
        Task ExecuteAsync(UpdateEventDto updateEventDto);
    }
}
