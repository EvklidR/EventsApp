using MediatR;
using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class RegisterUserForEventCommand : IRequest
    {
        public CreateProfileDto ProfileDto { get; set; }

        public RegisterUserForEventCommand(CreateProfileDto profileDto)
        {
            ProfileDto = profileDto;
        }
    }
}
