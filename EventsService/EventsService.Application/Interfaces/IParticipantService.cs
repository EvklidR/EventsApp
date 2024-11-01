using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces
{
    public interface IParticipantService
    {
        Task RegisterUserForEventAsync(CreateProfileDto profileDto);
        Task UnregisterUserFromEventAsync(int eventId, int userId);
        IEnumerable<EventDto> GetUserEvents(int userId);
        Task<ParticipantOfEventDto?> GetParticipantByIdAsync(int participantId);
        Task<IEnumerable<ParticipantOfEventDto>> GetParticipantsByEventIdAsync(int eventId);
    }
}
