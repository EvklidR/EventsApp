using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.ParticipantsUseCases
{
    public interface IGetParticipantById
    {
        Task<ParticipantOfEventDto> ExecuteAsync(int participantId);
    }
}
