using EventsService.Application.DTOs;
using MediatR;

namespace EventsService.Application.UseCases.EventsUseCases
{
    public class GetEventByIdQuery : IRequest<EventDto>
    {
        public GetEventByIdQuery(int id) 
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
