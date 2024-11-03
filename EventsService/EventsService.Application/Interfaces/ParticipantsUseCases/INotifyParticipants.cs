using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.ParticipantsUseCases
{
    public interface INotifyParticipants
    {
        Task ExecuteAsync(UpdateEventDto updateEventDto);
    }
}
