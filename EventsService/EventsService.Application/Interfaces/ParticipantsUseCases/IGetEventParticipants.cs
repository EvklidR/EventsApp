using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.ParticipantsUseCases
{
    public interface IGetEventParticipants
    {
        Task<IEnumerable<ParticipantOfEventDto>?> ExecuteAsync(int eventId);
    }
}
