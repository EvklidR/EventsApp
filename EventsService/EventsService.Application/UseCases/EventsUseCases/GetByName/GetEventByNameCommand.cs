using EventsService.Application.DTOs;
using MediatR;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetEventByNameCommand : IRequest<EventDto>
    {
        public string Name { get; set; }

        public GetEventByNameCommand(string name) 
        { 
            Name = name;
        }
    }
}
