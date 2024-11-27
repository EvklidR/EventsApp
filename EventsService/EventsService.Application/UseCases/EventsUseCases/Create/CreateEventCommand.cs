using MediatR;
using EventsService.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class CreateEventCommand : IRequest<EventDto>
    {
        public CreateEventCommand(CreateEventDto createEventDto, IFormFile? imageFile) 
        {
            EventDto = createEventDto;
            ImageFile = imageFile;
        }


        public CreateEventDto EventDto { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
