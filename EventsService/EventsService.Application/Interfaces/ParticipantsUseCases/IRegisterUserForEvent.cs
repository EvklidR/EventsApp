using EventsService.Application.DTOs;

namespace EventsService.Application.Interfaces.ParticipantsUseCases
{
    public interface IRegisterUserForEvent
    {
        Task ExecuteAsync(CreateProfileDto profileDto);
    }
}
