using EventsService.Application.DTOs;
using EventsService.Application.Interfaces;
using EventsService.Application.Interfaces.EventsUseCases;
using EventsService.Application.Interfaces.ParticipantsUseCases;

namespace EventsService.Application
{
    public class ParticipantsUseCasesFacade
    {
        private readonly IRegisterUserForEvent _registerUserForEventHandler;
        private readonly IUnregisterUserFromEvent _unregisterUserFromEventHandler;
        private readonly IGetParticipantById _getParticipantByIdHandler;
        private readonly IGetEventParticipants _getEventParticipantsHandler;

        public ParticipantsUseCasesFacade(
            IRegisterUserForEvent registerUserForEvent,
            IUnregisterUserFromEvent unregisterUserFromEvent,
            IGetParticipantById getParticipantById,
            IGetEventParticipants getParticipantsByEventId)
        {
            _registerUserForEventHandler = registerUserForEvent;
            _unregisterUserFromEventHandler = unregisterUserFromEvent;
            _getParticipantByIdHandler = getParticipantById;
            _getEventParticipantsHandler = getParticipantsByEventId;
        }

        public async Task RegisterUserForEventAsync(CreateProfileDto profileDto)
        {
            await _registerUserForEventHandler.ExecuteAsync(profileDto);
        }

        public async Task UnregisterUserFromEventAsync(int eventId, int userId)
        {
            await _unregisterUserFromEventHandler.ExecuteAsync(eventId, userId);
        }

        public async Task<ParticipantOfEventDto> GetParticipantByIdAsync(int participantId)
        {
            return await _getParticipantByIdHandler.ExecuteAsync(participantId);
        }

        public async Task<IEnumerable<ParticipantOfEventDto>?> GetEventParticipantsAsync(int eventId)
        {
            return await _getEventParticipantsHandler.ExecuteAsync(eventId);
        }
    }
}
