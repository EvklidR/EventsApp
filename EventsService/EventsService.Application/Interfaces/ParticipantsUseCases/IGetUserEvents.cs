using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.ParticipantsUseCases
{
    public interface IGetUserEvents
    {
        IEnumerable<EventDto>? Execute(int userId);
    }
}
