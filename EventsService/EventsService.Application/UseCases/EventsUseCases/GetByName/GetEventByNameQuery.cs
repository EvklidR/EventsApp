using EventsService.Application.DTOs;
using MediatR;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetEventByNameQuery : IRequest<EventDto>
    {
        public string Name { get; set; }

        public GetEventByNameQuery(string name) 
        { 
            Name = name;
        }
    }
}
