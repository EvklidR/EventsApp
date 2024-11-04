using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.EventsUseCases
{
    public interface IGetUserEvents
    {
        Task<IEnumerable<EventDto>?> ExecuteAsync(int userId);
    }
}
