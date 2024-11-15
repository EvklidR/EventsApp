using MediatR;
using EventsService.Application.DTOs;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class CreateEventCommand : IRequest<EventDto>
    {
        public CreateEventCommand(CreateEventDto createEventDto, string? imageFile) 
        {
            EventDto = createEventDto;
            ImageFile = imageFile;
        }

        public CreateEventDto EventDto { get; set; }
        public string? ImageFile { get; set; }
    }
}
