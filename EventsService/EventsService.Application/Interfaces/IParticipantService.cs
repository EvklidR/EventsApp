using EventsApp.EventsService.Application.DTOs;
using EventsApp.EventsService.Domain.Entities;

namespace EventsApp.EventsService.Application.Interfaces
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
