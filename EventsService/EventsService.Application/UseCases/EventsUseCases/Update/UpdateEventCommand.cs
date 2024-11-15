using MediatR;
using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class UpdateEventCommand : IRequest
    {
        public UpdateEventCommand(UpdateEventDto updateEventDto) 
        {
            UpdateEventDto = updateEventDto;
        }
        public UpdateEventDto UpdateEventDto { get; set; }
    }
}
