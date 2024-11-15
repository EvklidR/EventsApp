using MediatR;
using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.ParticipantsUseCases
{
    public class NotifyParticipantsCommand : IRequest
    {
        public UpdateEventDto UpdateEventDto { get; set; }

        public NotifyParticipantsCommand(UpdateEventDto updateEventDto)
        {
            UpdateEventDto = updateEventDto;
        }
    }
}
